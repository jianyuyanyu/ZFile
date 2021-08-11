using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.Interfaces
{
    public class Log : ILog
    {
        private static NLog.Logger logger;
        public Log()
        {
            logger = LogManager.GetCurrentClassLogger();
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="msg"></param>
        public void Error(string msg)
        {
            logger.Error(msg);
        }

        /// <summary>
        /// 错误-异常
        /// </summary>
        /// <param name="ex"></param>
        public void Error(Exception ex)
        {
            logger.Error(ex);
        }

        /// <summary>
        /// 输出信息
        /// </summary>
        /// <param name="msg"></param>
        public void Info(string msg)
        {
            logger.Info(msg);
        }

        /// <summary>
        /// 测试信息
        /// </summary>
        /// <param name="msg"></param>
        public void Debug(string msg)
        {
            logger.Debug(msg);
        }
    }
}
