using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XamarinCommom.Api;

namespace ZFileXamarin.Services
{
   public interface ILoginService
    {
        Task<BaseResponse> LoginAsync(string account, string passWord);
    }

    public class LoginService: ILoginService
    {
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
                Method = Method.POST
            }, false);
        }
    }

    public class UserLoginRequest : BaseRequest
    {
        public override string route { get => "api/Admin"; }
    }
}
