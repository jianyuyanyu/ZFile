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
using ZFile.Service.Interfaces.Sys;

namespace ZFileApiServer.Controllers.Admin
{
    [Produces("application/json")]
    [Route("api/Organize")]
    [JwtAuthorize(Roles = "Admin")]
    public class OrganizeController : ControllerBase
    {
        private readonly ISysOrganizeService _sysOrganizeService;
        public OrganizeController(ISysOrganizeService sysOrganizeService)
        {
            _sysOrganizeService = sysOrganizeService;
        }
        /// <summary>
        /// 获得组织结构Tree列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("gettree"), ApiAuthorize(Modules = "Department", Methods = "Add", LogType = LogEnum.RETRIEVE)]
        public async Task<IActionResult> GetListPage()
        {
            return Ok(await _sysOrganizeService.GetListTreeAsync());
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("getpages")]
        public async Task<IActionResult> GetPages([FromQuery] PageParm parm)
        {
            var res = await _sysOrganizeService.GetPagesAsync(parm);
            
            return Ok(new { code = 0, msg = "success", count = res.data.TotalItems, data = res.data.Items });
        }

        /// <summary>
        /// 获得字典栏目Tree列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("add"), ApiAuthorize(Modules = "Department", Methods = "Add", LogType = LogEnum.ADD)]
        public async Task<IActionResult> AddOrganize([FromBody] SysOrganize parm)
        {
            return Ok(await _sysOrganizeService.AddAsync(parm));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpPost("delete"), ApiAuthorize(Modules = "Department", Methods = "Delete", LogType = LogEnum.DELETE)]
        public async Task<IActionResult> DeleteOrganize([FromBody] ParmString parm)
        {
            return Ok(await _sysOrganizeService.DeleOrgnizeAsync(parm.parm));
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        [HttpPost("edit"), ApiAuthorize(Modules = "Department", Methods = "edit", LogType = LogEnum.UPDATE)]
        public async Task<IActionResult> EditOrganize([FromBody] SysOrganize parm)
        {
            return Ok(await _sysOrganizeService.ModifyAsync(parm));
        }

     
    }
}
