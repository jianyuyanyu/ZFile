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
        [HttpGet]
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
            var UserToken = await _sysAdmin.GetModelAsync(o => o.ID == int.Parse(token.Uid));
            UserInfo info = new UserInfo();
            //info.UserRealName = UserToken.data.UserRealName;
            info.username = UserToken.data.username;
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
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserModelDto model)
        {
            var apiRes = new ApiResult<string>() { statusCode = (int)ApiEnum.HttpRequestError };
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
                    apiRes.message = "您没有权限查看全部职员，请按部门查找";
                    return Ok(apiRes);
                }
                
                var user = dbres.data;
                
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
                    Uid = user.User.ID.ToString(),
                    UserName = user.User.username,
                    Role = "Admin",
                    TokenType = "Web"
                });
                apiRes.statusCode = (int)ApiEnum.Status;
                apiRes.data = token;
            }
            catch (Exception )
            {


            }
            return Ok(apiRes);
        }


        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> AddUserInfo([FromBody] AddUserDto user)
        {
            await Task.Delay(1);
            return Ok();
        }
    }
}
