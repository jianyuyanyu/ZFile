using FFmpeg.AutoGen;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        public void Createcontext()
        {

        }

        public VideoInfo OpenViDeo(string Path)
        {
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

           
            return null;
        }

    }
}
