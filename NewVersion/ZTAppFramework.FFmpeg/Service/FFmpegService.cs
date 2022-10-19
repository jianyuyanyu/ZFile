using FFmpeg.AutoGen;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZTAppFramework.FFmpeg.FFmpeg;
using ZTAppFramework.FFmpeg.Model;

namespace ZTAppFramework.FFmpeg.Service
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  ZT
    /// 创建时间    ：  2022/10/19 16:16:53 
    /// Description   ：  
    ///********************************************/
    /// </summary>
    public class FFmpegService
    {
        FFmpegLibrary fFmpegLibrary = new FFmpegLibrary();
        VideoInfo videoInfo;
        public FFmpegService()
        {
            fFmpegLibrary.RegisterFFmpegBinaries();
        }

      
    }
}
