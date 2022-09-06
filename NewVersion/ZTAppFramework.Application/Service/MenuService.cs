using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZTAppFramewrok.Application.Stared.DTO;
using ZTAppFramewrok.Application.Stared.HttpManager;
using ZTAppFreamework.Stared.Attributes;

namespace ZTAppFramework.Application.Service
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  ZT
    /// 创建时间    ：  2022/9/6 10:30:32 
    /// Description   ：  
    ///********************************************/
    /// </summary>
    public class MenuService : AppServiceBase
    {

        public override string ApiServiceUrl => "api/Menu";
        public MenuService(ApiClinetRepository apiClinet) : base(apiClinet)
        {

        }

        /// <summary>
        /// 获取菜单信息
        /// </summary>
        /// <returns></returns>
        [ApiUrl("authmenu")]
        public async Task<AppliResult<List<SysMenuDto>>> GetMenuList()
        {
            AppliResult<List<SysMenuDto>> res = new AppliResult<List<SysMenuDto>>() { Success = false, Message = "未知异常",data=new List<SysMenuDto>() };

            var api = await _apiClinet.GetAsync<List<SysMenuDto>>(GetEndpoint());
            if (api.success)
            {
                foreach (var item in api.data)
                {
                    var info= res.data.FirstOrDefault(x => x.name == item.parentName);
                    if (info != null)
                        info.Childer.Add(item);
                    else
                        res.data.Add(item);
                }
                res.Success = true;
                res.Message = api.message;
            }
            return res;
        }
    }
}
