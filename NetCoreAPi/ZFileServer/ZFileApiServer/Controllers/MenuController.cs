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
    [Route("api/Menu")]
    [JwtAuthorize(Roles = "Admin")]
    public class MenuController : ControllerBase
    {
        List<MenuDtio> SysMenuDtio;

        private readonly ISysAdminService _sysAdmin;
        public MenuController(ISysAdminService sysAdmin)
        {
            _sysAdmin = sysAdmin;
            if (SysMenuDtio==null)
            {
                SysMenuDtio = new List<MenuDtio>() {
                     new MenuDtio(){Id=26,ModelName="系统首页",ModelType="系统设置",ModelUrl="",ModelCode="XMGL",ComId=0,ORDERID=1,IsSys=1,WXUrl="",IsKJFS=0,PModelCode="XT",Token="",FunDatas=new List<FunData>(){
                        new FunData(){ID=1,ModelID=27,PageName="系统首页",ExtData="",PageUrl="",FunOrder="",PageCode="",isiframe="" }
                     } },
                     new MenuDtio(){Id=35,ModelName="系统配置",ModelType="系统设置",ModelUrl="",ModelCode="XMGL",ComId=0,ORDERID=1,IsSys=1,WXUrl="",IsKJFS=0,PModelCode="XT",Token="",FunDatas=new List<FunData>(){
                        new FunData(){ID=2,ModelID=27,PageName="系统配置",ExtData="",PageUrl="",FunOrder="",PageCode="",isiframe="" }
                     } },
                      new MenuDtio(){Id=27,ModelName="空间管理",ModelType="系统设置",ModelUrl="",ModelCode="KJGL",ComId=0,ORDERID=1,IsSys=1,WXUrl="",IsKJFS=0,PModelCode="XT",Token="",FunDatas=new List<FunData>(){
                        new FunData(){ID=3,ModelID=27,PageName="空间管理",ExtData="",PageUrl="",FunOrder="",PageCode="",isiframe="" }
                     } },
                       new MenuDtio(){Id=32,ModelName="用户管理",ModelType="系统设置",ModelUrl="",ModelCode="YHGL",ComId=0,ORDERID=1,IsSys=1,WXUrl="",IsKJFS=0,PModelCode="XT",Token="",FunDatas=new List<FunData>(){
                        new FunData(){ID=4,ModelID=27,PageName="用户管理",ExtData="",PageUrl="",FunOrder="",PageCode="",isiframe="" }
                     } },
                        new MenuDtio(){Id=28,ModelName="文件管理",ModelType="系统设置",ModelUrl="",ModelCode="WJGL",ComId=0,ORDERID=1,IsSys=1,WXUrl="",IsKJFS=0,PModelCode="XT",Token="",FunDatas=new List<FunData>(){
                        new FunData(){ID=5,ModelID=27,PageName="文件管理",ExtData="",PageUrl="",FunOrder="",PageCode="",isiframe="" }
                     } },
                         new MenuDtio(){Id=29,ModelName="日志管理",ModelType="系统设置",ModelUrl="",ModelCode="RZGL",ComId=0,ORDERID=1,IsSys=1,WXUrl="",IsKJFS=0,PModelCode="XT",Token="",FunDatas=new List<FunData>(){
                        new FunData(){ID=6,ModelID=27,PageName="日志管理",ExtData="",PageUrl="",FunOrder="",PageCode="",isiframe="" }
                     } },
                        new MenuDtio(){Id=31,ModelName="企业空间管理",ModelType="系统设置",ModelUrl="",ModelCode="QYKJGL",ComId=0,ORDERID=1,IsSys=1,WXUrl="",IsKJFS=0,PModelCode="XT",Token="",FunDatas=new List<FunData>(){
                        new FunData(){ID=7,ModelID=27,PageName="企业空间管理",ExtData="",PageUrl="",FunOrder="",PageCode="",isiframe="" }
                     } },
                        new MenuDtio(){Id=30,ModelName="个人空间",ModelType="工作台",ModelUrl="",ModelCode="GRKJ",ComId=0,ORDERID=1,IsSys=1,WXUrl="",IsKJFS=0,PModelCode="WORK",Token="",FunDatas=new List<FunData>(){
                        new FunData(){ID=8,ModelID=27,PageName="个人空间",ExtData="",PageUrl="",FunOrder="",PageCode="",isiframe="" }
                     } },
                        new MenuDtio(){Id=33,ModelName="企业空间",ModelType="工作台",ModelUrl="",ModelCode="QYKJ",ComId=0,ORDERID=1,IsSys=1,WXUrl="",IsKJFS=0,PModelCode="WORK",Token="",FunDatas=new List<FunData>(){
                        new FunData(){ID=9,ModelID=27,PageName="企业空间",ExtData="",PageUrl="",FunOrder="",PageCode="",isiframe="" }
                     } },
                        new MenuDtio(){Id=34,ModelName="用户共享",ModelType="工作台",ModelUrl="",ModelCode="YHGX",ComId=0,ORDERID=1,IsSys=1,WXUrl="",IsKJFS=0,PModelCode="WORK",Token="",FunDatas=new List<FunData>(){
                        new FunData(){ID=10,ModelID=27,PageName="用户共享",ExtData="",PageUrl="",FunOrder="",PageCode="",isiframe="" }
                     } },

                       
            };
            }

        }
        /// <summary>
        /// 查询菜单
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetMenu()
        {


            var apiRes = new ApiResult<List<MenuDtio>>() { statusCode = (int)ApiEnum.Status };

           
            string[] accesToken = Request.Headers["Authorization"].ToString().Split(' ');
            TokenModel token = JwtHelper.SerializeJWT(accesToken[1]);
            var UserInfo =  await _sysAdmin.GetModelAsync(o => o.ID == int.Parse(token.Uid));


         
            if (UserInfo.data.UserRealName!="超级管理员")
            {
                apiRes.data = SysMenuDtio.Where(o=>o.PModelCode== "WORK").ToList();
            }
            else
            {
                apiRes.data = SysMenuDtio;
            }
          
           
            return  Ok(apiRes);



        }
    }

}
