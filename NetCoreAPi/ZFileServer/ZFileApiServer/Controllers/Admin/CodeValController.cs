﻿using Microsoft.AspNetCore.Mvc;
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
    [Route("api/code")]
    [JwtAuthorize(Roles = "Admin")]
    public class CodeValController : ControllerBase
    {

        private readonly SysCodeService _sysCodeService;
        public CodeValController(SysCodeService sysCodeService)
        {
            _sysCodeService = sysCodeService;
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("getpages")]
        public async Task<IActionResult> GetPages([FromQuery] SysCodePostPage request)
        {
            var res = await _sysCodeService.GetPagesAsync(request);
            return Ok(new { code = 0, msg = "success", count = res.data.TotalItems, data = res.data.Items });
        }

        /// <summary>
        /// 获得字典栏目Tree列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("add"), ApiAuthorize(Modules = "Key", Methods = "Add", LogType = LogEnum.ADD)]
        public async Task<IActionResult> AddCodeType([FromBody] SysCode parm)
        {
            return Ok(await _sysCodeService.AddAsync(parm));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpPost("delete"), ApiAuthorize(Modules = "Key", Methods = "Delete", LogType = LogEnum.DELETE)]
        public async Task<IActionResult> DeleteCode([FromBody] ParmString obj)
        {
            return Ok(await _sysCodeService.DeleteAsync(obj.parm));
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        [HttpPost("edit"), ApiAuthorize(Modules = "Key", Methods = "Update", LogType = LogEnum.UPDATE)]
        public async Task<IActionResult> EditCode([FromBody] SysCode parm)
        {
            return Ok(await _sysCodeService.ModifyAsync(parm));
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        [HttpPost("editstatus"), ApiAuthorize(Modules = "Key", Methods = "Audit", LogType = LogEnum.AUDIT)]
        public async Task<IActionResult> EditStatusCode([FromBody] SysCode parm)
        {
            return Ok(await _sysCodeService.ModifyStatusAsync(parm));
        }
    }
}
