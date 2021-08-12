using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.Common.Helpers
{
   public class ByteConvert
    {

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string GetSize(long b)
        {
            if (b.ToString().Length <= 10)
                return GetMB(b);
            if (b.ToString().Length >= 11 && b.ToString().Length <= 12)
                return GetGB(b);
            if (b.ToString().Length >= 13)
                return GetTB(b);
            return String.Empty;
        }

        /// <summary>
        /// 将B转换为TB
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        private static string GetTB(long b)
        {
            for (int i = 0; i < 4; i++)
            {
                b /= 1024;
            }
            return b + "TB";
        }

        /// <summary>
        /// 将B转换为GB
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        private static string GetGB(long b)
        {
            for (int i = 0; i < 3; i++)
            {
                b /= 1024;
            }
            return b + "GB";
        }

        /// <summary>
        /// 将B转换为MB
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        private static string GetMB(long b)
        {
            for (int i = 0; i < 2; i++)
            {
                b /= 1024;
            }
            return b + "MB";
        }

        public static DateTime LastSpeedCheck;
    
        public static string GetSpeed(int LastSize, int CurrnetSize,int Size,ref TimeSpan timeSpan)
        {
            const decimal KB = 1024;
            const decimal MB = KB * 1024;
            const decimal GB = MB * 1024;
            long speed = (CurrnetSize- LastSize) *TimeSpan.TicksPerSecond/ (Time.Now.Ticks - LastSpeedCheck.Ticks);
            LastSize = CurrnetSize;
            LastSpeedCheck = Time.Now;
            timeSpan= GetRemainingTime(Size, LastSize);
            if (speed > GB)
                return $"{speed / GB:#,##0.0}GB/s";
            else if (speed > MB)
                return  $"{speed / MB:#,##0.0}MB/s";
            else if (speed > KB)
                return  $"{speed / KB:#,##0}KB/s";
            else
                return  $"{speed:#,##0}B/s";
        }
        public static TimeSpan GetRemainingTime(int Size,int Speed)
        {
            if (Speed == 0) Speed = 1;
         
           return TimeSpan.FromSeconds(Size / Speed); ;
        }
    }

    public static class Time
    {
        private static readonly DateTime StartTime = DateTime.Now;
        private static readonly Stopwatch Stopwatch = Stopwatch.StartNew();
        public static DateTime Now => StartTime + Stopwatch.Elapsed;
    }
}
