using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZT.Application.Operator
{
    /// <summary>
    /// 工作台
    /// </summary>
    [ApiExplorerSettings(GroupName = "v1")]
    public class WorkbenchService : IApplicationService
    {

        /// <summary>
        /// 获得资源使情况
        /// </summary>
        /// <returns></returns>
        public DeviceUse GetMachineUse()
        {
            return DeviceUtils.GetInstance().GetMachineUseInfo();
        }

    }
}
