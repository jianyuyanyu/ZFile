using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ZTAppFramewrok.Application.Stared.DTO;
using ZTAppFramewrok.Application.Stared.HttpManager;
using ZTAppFreamework.Stared.Attributes;

namespace ZTAppFramework.Application.Service
{
    public class UserService: AppServiceBase
    {
        public override string ApiServiceUrl => "/api/Admin";
        public UserService(ApiClinetRepository apiClinet) : base(apiClinet)
        {

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
            var api = await _apiClinet.PostAnonymousAsync<AppliResult<string>>(GetEndpoint(), user);
            if (api.Success)
            {
                if (api.statusCode == 200)
                {
                    res.Success = true;
                }
                else
                {
                    res.Message = api.Message;
                }
            }
            return res;
        }

   
    }
}
