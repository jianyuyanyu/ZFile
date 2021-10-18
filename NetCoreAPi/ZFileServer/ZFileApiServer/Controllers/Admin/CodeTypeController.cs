using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZFile.Common.EnumHelper;
using ZFile.Core.Model.User;
using ZFile.Extensions.Authorize;
using ZFile.Service.DtoModel;
using ZFile.Service.Implements;

namespace ZFileApiServer.Controllers
{
    [Produces("application/json")]
    [Route("api/codetype")]
    [JwtAuthorize(Roles = "Admin")]
    public class CodeTypeController : ControllerBase
    {

        private readonly SysCodeTypeService _sysCodeTypeService;
        public CodeTypeController(SysCodeTypeService sysCodeTypeService)
        {
            _sysCodeTypeService = sysCodeTypeService;
        }

        /// <summary>
        /// 获得字典栏目Tree列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("gettree")]
        public async Task<IActionResult> GetListPage()
        {
            var res = await _sysCodeTypeService.GetListTreeAsync();
            return Ok(res.data);
        }

        /// <summary>
        /// 获得字典栏目Tree列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("add"), ApiAuthorize(Modules = "Key", Methods = "Add", LogType = LogEnum.ADD)]
        public async Task<IActionResult> AddCodeType([FromBody] SysCodeType parm)
        {
            return Ok(await _sysCodeTypeService.AddAsync(parm));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpPost("delete"), ApiAuthorize(Modules = "Key", Methods = "Delete", LogType = LogEnum.DELETE)]
        public async Task<IActionResult> DeleteCode([FromBody] ParmString obj)
        {
            return Ok(await _sysCodeTypeService.DeleteAsync(obj.parm));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpPost("edit"), ApiAuthorize(Modules = "Key", Methods = "Update", LogType = LogEnum.UPDATE)]
        public async Task<IActionResult> EditCode([FromBody] SysCodeType parm)
        {
            return Ok(await _sysCodeTypeService.ModifyAsync(parm));
        }
    }
}
