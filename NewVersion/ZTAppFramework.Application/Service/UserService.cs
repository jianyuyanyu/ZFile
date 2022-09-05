using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ZTAppFramework.SqliteCore.Implements;
using ZTAppFramewrok.Application.Stared.DTO;
using ZTAppFramewrok.Application.Stared.HttpManager;
using ZTAppFreamework.Stared.Attributes;

namespace ZTAppFramework.Application.Service
{
    public class UserService : AppServiceBase
    {
        private readonly UserLocalSerivce _userLocalSerivce;

        public override string ApiServiceUrl => "/api/Admin";
        public UserService(ApiClinetRepository apiClinet, UserLocalSerivce userLocalSerivce) : base(apiClinet)
        {
            _userLocalSerivce = userLocalSerivce;
        }


        public async Task<AppliResult<List<UserInfoDto>>> GetLocalAccountList()
        {
            AppliResult<List<UserInfoDto>> result = new AppliResult<List<UserInfoDto>>() { Success = false };
              var Csql= await _userLocalSerivce.GetListAsync();

            return result;
        }

        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [ApiUrl("Login")]
        public async Task<AppliResult<string>> LoginServer(UserInfoDto user)
        {
            AppliResult<string> res = new AppliResult<string>() { Success = false, Message = "未知异常" };
            ApiResult<object> api = await _apiClinet.PostAnonymousAsync<object>(GetEndpoint(), user);
            if (api.success)
            {
                if (api.statusCode == 200)
                {
                    res.Success = true;
                    var info = await _userLocalSerivce.GetModelAsync(x => x.Name == user.User);
                    if (info.data==null)
                    {
                        var CSql = await _userLocalSerivce.AddAsync(new SqliteCore.Models.Account() { Name = user.User, Password = user.Password });
                    }
                }
                else
                {
                    res.Message = api.message;
                }
            }
            return res;
        }
    }
}
