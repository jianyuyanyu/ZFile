using Component;
using Component.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZFileWPFClinet.Service
{
    public interface ILoginService
    {
        Task<BaseResponse> LoginAsync(string account, string passWord);

        Task<BaseResponse<UserInfoDto>> GetUserInfoAsync();

        Task<BaseResponse> CheckVersion(string productName, string version);
    }
}
