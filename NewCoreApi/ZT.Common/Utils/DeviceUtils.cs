using Masuit.Tools.Hardware;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ZT.Common.Utils
{
            {
                var output = ShellUtil.Bash("top -l 1 | head -n 10");
                var lines = output.Split("\n");
                var memory = lines[6].Split(" ", StringSplitOptions.RemoveEmptyEntries);
                var allMemoryStr = memory[1].Replace("G", "");
                var useMemory = memory[3].Replace("(", "").Replace("M", "");
                var free = double.Parse(allMemoryStr) * 1024 / double.Parse(useMemory);
                return new MemoryInfo
                {
                    Total = double.Parse(allMemoryStr) * 1024,
                    Used = double.Parse(useMemory),
                    Free = free
                };
            }
            if (IsUnix())
            {
                var output = ShellUtil.Bash("free -m");
                var lines = output.Split("\n");
                var memory = lines[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);
                return new MemoryInfo
                {
                    Total = double.Parse(memory[1]),
                    Used = double.Parse(memory[2]),
                    Free = double.Parse(memory[3])
                };
            }
            else
            {
                var output = ShellUtil.Cmd("wmic", "OS get FreePhysicalMemory,TotalVisibleMemorySize /Value");
                var lines = output.Trim().Split("\n");
                var freeMemoryParts = lines[0].Split("=", StringSplitOptions.RemoveEmptyEntries);
                var totalMemoryParts = lines[1].Split("=", StringSplitOptions.RemoveEmptyEntries);
                var total = Math.Round(double.Parse(totalMemoryParts[1]) / 1024, 2);
                var free = Math.Round(double.Parse(freeMemoryParts[1]) / 1024, 2);
                return new MemoryInfo
                {
                    Total = total,
                    Free = free,
                    Used = total - free
                };
            }
        }

        /// <summary>
        /// 获取外网IP和地理位置
        /// </summary>
        /// <returns></returns>
        private async Task<string> GetWanIpFromPCOnline()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var url = "http://whois.pconline.com.cn/ipJson.jsp";
            var stream = await HttpUtils.HttpGetAsync(url);
            var streamReader = new StreamReader(stream, Encoding.GetEncoding("GBK"));
            var html = await streamReader.ReadToEndAsync();
            var tmp = html[(html.IndexOf("({", StringComparison.Ordinal) + 2)..].Split(",");
            var ipAddr = tmp[0].Split(":")[1] + "【" + tmp[7].Split(":")[1] + "】";
            return ipAddr.Replace("\"", "");
        }

        /// <summary>
        /// 获取系统运行时间
        /// </summary>
        /// <returns></returns>
        private string GetRunTime()
        {
            return FormatTime((long)(DateTimeOffset.Now - Process.GetCurrentProcess().StartTime).TotalMilliseconds);
        }

        /// <summary>
        /// 毫秒转天时分秒
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        private string FormatTime(long ms)
        {
            int ss = 1000;
            int mi = ss * 60;
            int hh = mi * 60;
            int dd = hh * 24;

            long day = ms / dd;
            long hour = (ms - day * dd) / hh;
            long minute = (ms - day * dd - hour * hh) / mi;
            long second = (ms - day * dd - hour * hh - minute * mi) / ss;
            //long milliSecond = ms - day * dd - hour * hh - minute * mi - second * ss;

            string sDay = day < 10 ? "0" + day : "" + day; //天
            string sHour = hour < 10 ? "0" + hour : "" + hour;//小时
            string sMinute = minute < 10 ? "0" + minute : "" + minute;//分钟
            string sSecond = second < 10 ? "0" + second : "" + second;//秒
                                                                      //string sMilliSecond = milliSecond < 10 ? "0" + milliSecond : "" + milliSecond;//毫秒
                                                                      //sMilliSecond = milliSecond < 100 ? "0" + sMilliSecond : "" + sMilliSecond;
            return $"{sDay} 天 {sHour} 小时 {sMinute} 分 {sSecond} 秒";
        }
    }

    public class DeviceUse
    {
        public string TotalRam { get; set; }

        public double RamRate { get; set; }

        public double CpuRate { get; set; }

        public double DiskRate { get; set; }

        public string RunTime { get; set; }

        /// <summary>
        /// 网络上行
        /// </summary>
        public long NetWorkUp { get; set; }

        /// <summary>
        /// 网络下行
        /// </summary>
        public long NetWorkDown { get; set; }
    }

    public class MemoryInfo
    {
        public double Total { get; set; }

        public double Used { get; set; }

        public double Free { get; set; }
    }

    /// <summary>
    /// 系统Shell命令
    /// </summary>
    public static class ShellUtil
    {
        /// <summary>
        /// Bash命令
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static string Bash(string command)
        {
            var escapedArgs = command.Replace("\"", "\\\"");
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            var result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            process.Dispose();
            return result;
        }

        /// <summary>
        /// cmd命令
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string Cmd(string fileName, string args)
        {
            string output = null;
            var info = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = args,
                RedirectStandardOutput = true
            };
            using var process = Process.Start(info);
            if (process != null) output = process.StandardOutput.ReadToEnd();
            return output;
        }
    }
}
