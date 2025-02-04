﻿using Microsoft.AspNetCore.Authentication;
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
using ZFile.Common.Cache;
using ZFile.Common.ConfigHelper;
using ZFile.Common.EnumHelper;
using ZFile.Core.Model.User;
using ZFile.Extensions.Authorize;
using ZFile.Extensions.JWT;
using ZFile.Service.DtoModel;
using ZFile.Service.Interfaces;

namespace ZFileApiServer.Controllers
{
    [Produces("application/json")]
    [Route("api/Admin")]
    [JwtAuthorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly ILogger<AdminController> _logger;
        private readonly ISysAdminService _sysAdmin;
        private readonly ISysAppseting _sysAppseting;
        private readonly IFileQycodeService _fileQycode;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="sysAdmin"></param>
        /// <param name="sysAppseting"></param>
        /// <param name="fileQycode"></param>
        public AdminController(ILogger<AdminController> logger, ISysAdminService sysAdmin, ISysAppseting sysAppseting, IFileQycodeService fileQycode)
        {
            _logger = logger;
            _sysAdmin = sysAdmin;
            _sysAppseting = sysAppseting;
            _fileQycode = fileQycode;
        }

        /// <summary>
        /// 获取用户设置信息
        /// </summary>
        /// <returns></returns>     
        
        [HttpGet("Get"), ApiAuthorize(Modules = "Admin", Methods = "Add", LogType = LogEnum.ADD)]
        public async Task<IActionResult> GetUserSysInfo()
        {
            var apiRes = new ApiResult<Msg_Result>() { statusCode = (int)ApiEnum.HttpRequestError };
            string[] accesToken = Request.Headers["Authorization"].ToString().Split(' ');
            TokenModel token = JwtHelper.SerializeJWT(accesToken[1]);
            var res = await _fileQycode.GetALLEntities();
            Msg_Result msg = new Msg_Result();
            JH_Auth_QYDto QYinfo = new JH_Auth_QYDto();
            QYinfo.QYCode = res.data.Code;
            QYinfo.FileServerUrl = HttpContext.Connection.LocalIpAddress + ":" + HttpContext.Connection.LocalPort;
            var UserToken = await _sysAdmin.GetModelAsync(o => o.Guid == token.Uid);
            SysAdmin info = new SysAdmin();
            //info.UserRealName = UserToken.data.UserRealName;
            info.LoginName = UserToken.data.LoginName;
            //info.Role = token.Role;
            msg.Result = info;
            msg.Result1 = _sysAppseting.GetValueByKey("sysname").Result.data;
            msg.Result2 = _sysAppseting.GetValueByKey("qyname").Result.data;
            msg.Result3 = _sysAppseting.GetValueByKey("qyico").Result.data;
            msg.Result4 = res.data;
            apiRes.statusCode = (int)ApiEnum.Status;
            apiRes.data = msg;
            return Ok(apiRes);
        }

        /// <summary>
        /// 用户登入
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserModelDto model)
        {
            ApiResult<AuthenticateDto> apiRes = new ApiResult<AuthenticateDto>() { statusCode = (int)ApiEnum.HttpRequestError };
            var token = "";
            try
            {
                var dbres = await _sysAdmin.LoginAsync(model.User, model.Password);
                if (dbres.statusCode == (int)ApiEnum.Error)
                {
                    apiRes.message = "账号密码错误！！";
                    return Ok(apiRes);
                }
                else if (dbres.statusCode == (int)ApiEnum.Unauthorized)
                {
                    apiRes.message = "账号密码错误！！";
                    return Ok(apiRes);
                }          
                var user = dbres.data;
                MemoryCacheService.Default.SetCache(KeyHelper.ADMINMENU + "_" + dbres.data.admin.Guid, dbres.data.menu, 600);

                #region 废弃


                // var identity = new ClaimsPrincipal(
                // new ClaimsIdentity(new[]
                //     {
                //                new Claim(ClaimTypes.Role,user.User.Role),
                //                new Claim(ClaimTypes.Name,user.User.username),
                //                new Claim(ClaimTypes.WindowsAccountName,user.User.UserRealName),
                //                new Claim(ClaimTypes.Expiration,DateTime.UtcNow.AddSeconds(12).ToString())
                //     }, CookieAuthenticationDefaults.AuthenticationScheme)
                //);

                ////如果保存用户类型是Session，则默认设置cookie退出浏览器 清空
                //if (ConfigExtensions.Configuration[KeyHelper.LOGINSAVEUSER] == "Session")
                //{
                //    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, identity, new AuthenticationProperties
                //    {
                //        AllowRefresh = false
                //    });
                //}
                //else
                //{
                //    //根据配置保存浏览器用户信息，小时单位
                // var hours = int.Parse(ConfigExtensions.Configuration[KeyHelper.LOGINCOOKIEEXPIRES]);
                //    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, identity, new AuthenticationProperties
                //    {
                //        ExpiresUtc = DateTime.UtcNow.AddSeconds(10),
                //        IsPersistent = true,
                //        AllowRefresh = false
                //    });
                //}
                #endregion

                token = JwtHelper.IssueJWT(new TokenModel()
                {
                    Uid = user.admin.Guid.ToString(),
                    UserName = user.admin.LoginName,
                    Role = "Admin",
                    TokenType = "Web"
                });
                apiRes.statusCode = (int)ApiEnum.Status;
                AuthenticateDto dto = new AuthenticateDto()
                {
                    Token = token,
                    Role = "管理员",
                    TokenExpiration = DateTime.Now.AddMinutes(10).Minute,
                    RefreshToken = ""
                };
                apiRes.data = dto;
            }
            catch (Exception )
            {


            }
            return Ok(apiRes);
        }


        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("getpages")]
        public async Task<IActionResult> GetPages([FromQuery] PageParm parm)
        {
            var res = await _sysAdmin.GetPagesAsync(parm);
            return Ok(new { code = 0, msg = "success", count = res.data.TotalItems, data = res.data.Items });
        }

        /// <summary>
        /// 根据编号，查询用户信息
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost("bymodel")]
        public async Task<IActionResult> GetModelByGuid([FromBody] ParmString parm)
        {
            var res = await _sysAdmin.GetModelAsync(m => m.Guid == parm.parm);
            if (!string.IsNullOrEmpty(res.data.Guid))
            {
                res.data.LoginPwd = Utils.GetMD5(res.data.LoginPwd) ;
            }
            return Ok(res);
        }

        [HttpPost("add"), ApiAuthorize(Modules = "Admin", Methods = "Add", LogType = LogEnum.ADD)]
        public async Task<IActionResult> AddUserInfo([FromBody] SysAdmin parm)
        {
            return Ok(await _sysAdmin.AddAsync(parm));
           
        }
      
      
    }
}
