
using Component;
using Component.Api;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZFileWPFClinet.Service
{
    public class LoginService : ILoginService
    {
        public async Task<BaseResponse> CheckVersion(string account, string passWord)
        {
            return await new BaseServiceRequest().GetRequest<BaseResponse>(new UserLoginRequest()
            {
                parameters = new Dictionary<string, object>()
                {
                    {"",new {
                        account = account,
                        passWord = passWord,
                    }}
                },
                Method = Method.POST
            });
        }

        public async Task<BaseResponse<UserInfoDto>> GetUserInfoAsync()
        {
            return await new BaseServiceRequest().GetRequest<BaseResponse<UserInfoDto>>(new UserInfoRequest() { IsJson=false}) ;
        }

        public async Task<BaseResponse> LoginAsync(string account, string passWord)
        {
            return await new BaseServiceRequest().GetRequest<BaseResponse>(new UserLoginRequest()
            {
                parameters = new Dictionary<string, object>()
                {
                    {"",new {
                        user = account,
                        password = passWord,
                    }}
                },
                Method = Method.POST,
                IsJson=true
            },false);
        }
    }
    public class UserInfoRequest : BaseRequest
    {
        public override string route { get => "api/Admin"; }
    }
    public class UserLoginRequest : BaseRequest
    {
        public override string route { get => "api/Admin"; }
    }

    public class AuthItemRequest : BaseRequest
    {
        public override string route { get => "api/AuthItem/GetAll"; }
    }

    public class CheckVersionRequest : BaseRequest
    {
        public override string route { get => "api/UpdateLog/Check"; }
        public string productName { get; set; }
        public string version { get; set; }
    }
}
