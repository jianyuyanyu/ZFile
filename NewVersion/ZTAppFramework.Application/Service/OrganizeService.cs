using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZTAppFramewrok.Application.Stared.DTO;
using ZTAppFramewrok.Application.Stared.HttpManager;
using ZTAppFramewrok.Application.Stared.HttpManager.Model;
using ZTAppFreamework.Stared.Attributes;

namespace ZTAppFramework.Application.Service
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  ZT
    /// 创建时间      ：  2022/9/7 8:26:15 
    /// Description   ：  
    ///********************************************/
    /// </summary>
    public class OrganizeService : AppServiceBase
    {
        public OrganizeService(ApiClinetRepository apiClinet) : base(apiClinet)
        {


        }

        [ApiUrl("")]
        public async Task<AppliResult<List<SysOrganizeDTo>>> GetOrganizeList(PageParm Parm)
        {
            AppliResult<SysOrganizeDTo> result = new AppliResult<SysOrganizeDTo>();

            await Task.CompletedTask;
            return result;
        }

    }
}
