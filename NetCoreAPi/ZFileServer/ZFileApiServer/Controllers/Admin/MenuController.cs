using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ZFile.Common;
using ZFile.Common.ApiClient;
using ZFile.Common.ConfigHelper;
using ZFile.Common.EnumHelper;
using ZFile.Core.Model.User;
using ZFile.Extensions.Authorize;
using ZFile.Extensions.JWT;
using ZFile.Service.DtoModel;
using ZFile.Service.Implements;
using ZFile.Service.Interfaces;


namespace ZFileApiServer.Controllers
{
    [Produces("application/json")]
    [Route("api/Menu")]
    [JwtAuthorize(Roles = "Admin")]
    public class MenuController : ControllerBase
    {
    
        private readonly SysMenuService _sysMenuService;
        //private readonly SysAuthorizeService _authorizeService;
        //private readonly CacheService _cacheService;
        private readonly SysPermissionsService _sysPermissionsService;
        public MenuController(SysMenuService sysMenuService
            , SysPermissionsService sysPermissionsService)
        {
            _sysMenuService = sysMenuService;
         
           
            _sysPermissionsService = sysPermissionsService;
        }

       

        /// <summary>
        /// 获得组织结构Tree列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("gettree")]
        public async Task<IActionResult> GetListPage([FromBody] MenuTreeParm param)
        {
            var res = await _sysMenuService.GetListTreeAsync(param.roleGuid);
            return Ok(res.data);
        }

        /// <summary>
        /// 提供角色弹框授权返回客户端菜单列表和当前角色的列表
        /// 涉及到选中状态
        /// </summary>
        [HttpPost("menubyrole")]
        public async Task<IActionResult> GetMenuByRole([FromBody] MenuTreeParm param)
        {
            var menu = await _sysMenuService.GetListTreeAsync(param.roleGuid);
            var res = new MenuRoleDto()
            {
                menu = menu.data
            };
            return Ok(res);
        }
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("getpages")]
        public async Task<IActionResult> GetPages([FromQuery] PageParm parm)
        {
            var res = await _sysMenuService.GetPagesAsync(parm);
            if (res.data?.Items?.Count > 0)
            {
                foreach (var item in res.data.Items)
                {
                    item.Name = Utils.LevelName(item.Name, item.Layer);
                }
            }
            return Ok(new { code = 0, msg = "success", count = res.data?.TotalItems, data = res.data?.Items });
        }


        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <returns></returns>
        [HttpPost("add"), ApiAuthorize(Modules = "Menu", Methods = "Add", LogType = LogEnum.ADD)]
        public async Task<IActionResult> AddMenu([FromBody] SysMenu model)
        {
            return Ok(await _sysMenuService.AddAsync(model, model.cbks));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpPost("delete"), ApiAuthorize(Modules = "Menu", Methods = "Delete", LogType = LogEnum.DELETE)]
        public async Task<IActionResult> DeleteMenu([FromBody] ParmString obj)
        {
            var list = Utils.StrToListString(obj.parm);
            return Ok(await _sysMenuService.DeleteAsync(m => list.Contains(m.Guid)));
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        [HttpPost("edit"), ApiAuthorize(Modules = "Menu", Methods = "Update", LogType = LogEnum.UPDATE)]
        public async Task<IActionResult> EditMenu([FromBody] SysMenu model)
        {
            return Ok(await _sysMenuService.ModifyAsync(model, model.cbks));
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <returns></returns>
        [HttpPost("sort")]
        public async Task<IActionResult> ColStor([FromBody] ParmStringSort obj)
        {
            return Ok(await _sysMenuService.ColSort(obj.p, obj.i, obj.o));
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        [HttpPost("authorizaion")]
        [ApiAuthorize(Modules = "Menu", Methods = "Update", LogType = LogEnum.STATUS)]
        public async Task<IActionResult> GetAuthorizaionMenu([FromBody] ParmString obj)
        {
            return Ok(await _sysMenuService.GetMenuByRole(obj.parm));
        }
    }

}
