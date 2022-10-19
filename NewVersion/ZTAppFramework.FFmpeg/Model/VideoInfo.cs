using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTAppFramework.FFmpeg.Model
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  WeiXiaolei
    /// 创建时间    ：  2022/10/19 16:35:12 
    /// Description   ：  
    ///********************************************/
    /// </summary>
    public class VideoInfo
    {
        public TimeSpan Duration { get; set; }//视频时长

        public string CodecId { get; set; }
        public string CodecName { get; set; }
        public int Bitrate { get;  set; }
        public double FrameRate { get;  set; }
        public int FrameWidth { get;  set; }
        public int FrameHeight { get;  set; }
        public TimeSpan frameDuration { get;  set; }

        //Duration = Time
        //CodecId = video
        // CodecName = ffm
        // Bitrate = (int)
        // FrameRate = ffm
        // FrameWidth = vi
        // FrameHeight = v
        // frameDuration =
    }
}
