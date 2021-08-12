using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.Api
{
    public class BaseResponse
    {
        /// <summary>
        /// 后台消息
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// //返回状态
        /// </summary>
        public int statusCode { get; set; }

        public bool success { get; set; }

        public object data { get; set; }
    }

    public class BaseResponse<T>
    {
        /// <summary>
        /// 后台消息
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// //返回状态
        /// </summary>
        public int statusCode { get; set; }

        public bool success { get; set; }

        public T data { get; set; }
    }
}
