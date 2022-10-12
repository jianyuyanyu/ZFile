using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZTAppFramewrok.Application.Stared.HttpManager;

namespace ZTAppFramework.Application.Service
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  ZT
    /// 创建时间    ：  2022/10/12 10:23:13 
    /// Description   ：  日志服务
    ///********************************************/
    /// </summary>
    public class SysLogSerivce : AppServiceBase
    {

        public override string ApiServiceUrl => "";
        public SysLogSerivce(ApiClinetRepository apiClinet) : base(apiClinet)
        {

        }


    }
}
