using FFmpeg.AutoGen;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ZTAppFramework.FFmpeg.Model;

namespace ZTAppFramework.FFmpeg.FFmpeg
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  WeiXiaolei
    /// 创建时间    ：  2022/10/19 16:19:38 
    /// Description   ：  
    ///********************************************/
    /// </summary>
    public unsafe class FFmpegLibrary
    {
        public FFmpegLibrary()
        {

        }

        public void RegisterFFmpegBinaries()
        {
            //获取当前软件启动的位置
            var currentFolder = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            //ffmpeg在项目中放置的位置
            var probe = Path.Combine("FFmpeg", Environment.Is64BitOperatingSystem ? "x64" : "x86");
            while (currentFolder != null)
            {
                var ffmpegBinaryPath = Path.Combine(currentFolder, probe);
                if (Directory.Exists(ffmpegBinaryPath))
                {
                    //找到dll放置的目录，并赋值给rootPath;
                    ffmpeg.RootPath = ffmpegBinaryPath;
                    return;
                }
                currentFolder = Directory.GetParent(currentFolder)?.FullName;
            }
            //旧版本需要要调用这个方法来注册dll文件，新版本已经会自动注册了
            //ffmpeg.avdevice_register_all();
        }
        //媒体格式上下文（媒体容器）
        AVFormatContext* format;
        //编解码上下文
        AVCodecContext* codecContext;
        //媒体数据包
        AVPacket* packet;
        //媒体帧数据
        AVFrame* frame;
        //图像转换器
        SwsContext* convert;
        //视频流
        AVStream* videoStream;
        // 视频流在媒体容器上流的索引
        int videoStreamIndex;

        public IntPtr FrameBufferPtr;

        public byte_ptrArray4 TargetData;

        public int_array4 TargetLinesize;
        public VideoInfo OpenViDeo(string Path)
        {
            VideoInfo videoInfo = new VideoInfo();
            format = ffmpeg.avformat_alloc_context();
            if (format == null)
            {
                throw new Exception("创建媒体格式（容器）失败");
            }
            int error = 0;
            AVFormatContext* tempFormat = format;
            //打开视频
            error = ffmpeg.avformat_open_input(&tempFormat, Path, null, null);
            if (error < 0)
            {
                throw new Exception("打开视频失败");
            }
            //获取流信息
            ffmpeg.avformat_find_stream_info(format, null);
            //编解码器类型
            AVCodec* codec = null;
            //获取视频流索引
            videoStreamIndex = ffmpeg.av_find_best_stream(format, AVMediaType.AVMEDIA_TYPE_VIDEO, -1, -1, &codec, 0);
            if (videoStreamIndex < 0)
            {
                throw new Exception("没有找到视频流");
            }
            //根据流索引找到视频流
            videoStream = format->streams[videoStreamIndex];
            //创建解码器上下文
            codecContext = ffmpeg.avcodec_alloc_context3(codec);
            //将视频流里面的解码器参数设置到 解码器上下文中
            error = ffmpeg.avcodec_parameters_to_context(codecContext, videoStream->codecpar);
            if (error < 0)
            {
                throw new Exception("设置解码器参数失败");
             
            }
            //打开解码器
            error = ffmpeg.avcodec_open2(codecContext, codec, null);
            if (error < 0)
            {
                throw new Exception("打开解码器失败");
            }
            //视频时长等视频信息
           
            videoInfo.Duration = TimeSpan.FromMilliseconds(format->duration / 1000);
            videoInfo.CodecId = videoStream->codecpar->codec_id.ToString();
            videoInfo.CodecName = ffmpeg.avcodec_get_name(videoStream->codecpar->codec_id);
            videoInfo.Bitrate = (int)videoStream->codecpar->bit_rate;
            videoInfo.FrameRate = ffmpeg.av_q2d(videoStream->r_frame_rate);
            videoInfo.FrameWidth = videoStream->codecpar->width;
            videoInfo.FrameHeight = videoStream->codecpar->height;
            videoInfo.frameDuration = TimeSpan.FromMilliseconds(1000 / videoInfo.FrameRate);
            //初始化转换器，将图片从源格式 转换成 BGR0 （8:8:8）格式
            var result = InitConvert(videoInfo.FrameWidth, videoInfo.FrameHeight, codecContext->pix_fmt, videoInfo.FrameWidth, videoInfo.FrameHeight, AVPixelFormat.AV_PIX_FMT_BGR0);
            //所有内容都初始化成功了开启时钟，用来记录时间
            if (result)
            {
                //从内存中分配控件给 packet 和frame
                packet = ffmpeg.av_packet_alloc();
                frame = ffmpeg.av_frame_alloc();
            }
            return videoInfo;
        }

        bool InitConvert(int sourceWidth, int sourceHeight, AVPixelFormat sourceFormat, int targetWidth, int targetHeight, AVPixelFormat targetFormat)
        {
            //根据输入参数和输出参数初始化转换器
            convert = ffmpeg.sws_getContext(sourceWidth, sourceHeight, sourceFormat, targetWidth, targetHeight, targetFormat, ffmpeg.SWS_FAST_BILINEAR, null, null, null);
            if (convert == null)
            {
                Debug.WriteLine("创建转换器失败");
                return false;
            }
            //获取转换后图像的 缓冲区大小
            var bufferSize = ffmpeg.av_image_get_buffer_size(targetFormat, targetWidth, targetHeight, 1);
            //创建一个指针
            FrameBufferPtr = Marshal.AllocHGlobal(bufferSize);
            TargetData = new byte_ptrArray4();
            TargetLinesize = new int_array4();
            ffmpeg.av_image_fill_arrays(ref TargetData, ref TargetLinesize, (byte*)FrameBufferPtr, targetFormat, targetWidth, targetHeight, 1);
            return true;
        }

    }
}
