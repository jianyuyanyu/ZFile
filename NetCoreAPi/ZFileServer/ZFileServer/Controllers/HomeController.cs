using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ZFile.Common;
using ZFile.Common.ApiClient;
using ZFile.Common.ConfigHelper;
using ZFile.Common.EnumHelper;
using ZFile.Core.Model.File;
using ZFile.Extensions.JWT;
using ZFile.Service.DtoModel;
using ZFile.Service.Implements;
using ZFile.Service.Interfaces;
using ZFileServer.Code;

namespace ZFileServer.Controllers
{
    public class HomeController : WebBaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISysAdminService _sysAdmin;
        private readonly ISysAppseting _sysAppseting;
        private readonly IFileQycodeService _fileQycode;
        public HomeController(ILogger<HomeController> logger,ISysAdminService sysAdmin,ISysAppseting sysAppseting, IFileQycodeService fileQycode)
        {
            _logger = logger;
            _sysAdmin = sysAdmin;
            _sysAppseting = sysAppseting;
            _fileQycode = fileQycode;
        }

        public IActionResult Index()
        {
        
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult FileIndex()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Index([FromForm]UserModelDto model)
        {
            var apiRes = new ApiResult<Msg_Result>() { statusCode = (int)ApiEnum.HttpRequestError };
            var token = "";
            try
            {
                var dbres = await _sysAdmin.LoginAsync(model.User,model.Password);
                if (dbres.statusCode != 200)
                {
                      return MessageBoxView("您没有权限查看全部职员，请按部门查找");
                }

                var user = dbres.data;
                var identity = new ClaimsPrincipal(
                 new ClaimsIdentity(new[]
                     {
                              new Claim(ClaimTypes.Role,user.User.RoleGuid),
                              new Claim(ClaimTypes.Name,user.User.LoginName),
                              new Claim(ClaimTypes.WindowsAccountName,user.User.UserRealName),
                     }, CookieAuthenticationDefaults.AuthenticationScheme)
                );
                //如果保存用户类型是Session，则默认设置cookie退出浏览器 清空
                if (ConfigExtensions.Configuration[KeyHelper.LOGINSAVEUSER] == "Session")
                {
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, identity, new AuthenticationProperties
                    {
                        AllowRefresh = false
                    });
                }
                else
                {
                    //根据配置保存浏览器用户信息，小时单位
                    var hours = int.Parse(ConfigExtensions.Configuration[KeyHelper.LOGINCOOKIEEXPIRES]);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, identity, new AuthenticationProperties
                    {
                        ExpiresUtc = DateTime.UtcNow.AddSeconds(10),
                        IsPersistent = true,
                        AllowRefresh = false
                    });
                }
                var res = await _fileQycode.GetALLEntities();
                Msg_Result msg = new Msg_Result();
                JH_Auth_QYDto QYinfo = new JH_Auth_QYDto();
                QYinfo.QYCode= res.data.Code;
                QYinfo.FileServerUrl = HttpContext.Connection.LocalIpAddress + ":" + HttpContext.Connection.LocalPort;
                user.QYinfo = QYinfo;
                msg.Result = user;
                msg.Result1 = _sysAppseting.GetValueByKey("sysname").Result.data;
                msg.Result2 = _sysAppseting.GetValueByKey("qyname").Result.data;
                msg.Result3 = _sysAppseting.GetValueByKey("qyico").Result.data;
                msg.Result4 = res.data;
                apiRes.data = msg;
             
                return View("FileIndex", apiRes.data);

            }
            catch (Exception)
            {

                return View();
            }

            
           
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
