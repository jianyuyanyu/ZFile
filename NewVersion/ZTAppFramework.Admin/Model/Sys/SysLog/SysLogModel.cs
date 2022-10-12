using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTAppFramework.Admin.Model.Sys.SysLog
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  ZT
    /// 创建时间    ：  2022/10/12 11:07:04 
    /// Description   ：  
    ///********************************************/
    /// </summary>
    public class SysLogModel: BindableBase
    {
        /// <summary>
        /// 唯一编号
        /// </summary>
        private long _Id;
        public long Id
        {
            get { return _Id; }
            set { SetProperty(ref _Id, value); }
        }
        /// <summary>
        /// 日志级别
        /// </summary>
        private LogEnum _Level;
        public LogEnum Level
        {
            get { return _Level; }
            set { SetProperty(ref _Level, value); }
        }
        /// <summary>
        /// 日志类型  1=登录  2=操作
        /// </summary>
        private LogTypeEnum _LogType;
        public LogTypeEnum LogType
        {
            get { return _LogType; }
            set { SetProperty(ref _LogType, value); }
        }
        /// <summary>
        /// 操作模块
        /// </summary>
        private string _Module;
        public string Module
        {
            get { return _Module; }
            set { SetProperty(ref _Module, value); }
        }
        /// <summary>
        /// 操作类型:例如添加、修改
        /// </summary>
        private string _OperateType;
        public string OperateType
        {
            get { return _OperateType; }
            set { SetProperty(ref _OperateType, value); }
        }
        /// <summary>
        /// 提交类型：get/post/delete
        /// </summary>
        private string _Method;
        public string Method
        {
            get { return _Method; }
            set { SetProperty(ref _Method, value); }
        }
        /// <summary>
        /// 操作人
        /// </summary>
        private string _OperateUser;
        public string OperateUser
        {
            get { return _OperateUser; }
            set { SetProperty(ref _OperateUser, value); }
        }
        /// <summary>
        /// IP
        /// </summary>
        private string _IP;
        public string IP
        {
            get { return _IP; }
            set { SetProperty(ref _IP, value); }
        }
        /// <summary>
        /// 请求参数
        /// </summary>
        private string _Parameters;
        public string Parameters
        {
            get { return _Parameters; }
            set { SetProperty(ref _Parameters, value); }
        }
        /// <summary>
        /// 操作地址
        /// </summary>
        private string _Address;
        public string Address
        {
            get { return _Address; }
            set { SetProperty(ref _Address, value); }
        }
        /// <summary>
        /// 操作状态
        /// </summary>

        private bool _Status = true;
        public bool Status
        {
            get { return _Status; }
            set { SetProperty(ref _Status, value); }
        }
        /// <summary>
        /// 详细信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 返回结果
        /// </summary>
        public string ReturnValue { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>

        public DateTime OperateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 浏览器信息
        /// </summary>
        public string Browser { get; set; }

        /// <summary>
        /// 执行时长
        /// </summary>

        public int ExecutionDuration { get; set; } = 0;

    }
}
