using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using ZTAppFramework.SqliteCore.Implements;
using ZTAppFramework.SqliteCore.Models;
using ZTAppFramewrok.Application.Stared.DTO;
using ZTAppFramewrok.Application.Stared.HttpManager;
using ZTAppFreamework.Stared;
using ZTAppFreamework.Stared.Attributes;

namespace ZTAppFramework.Application.Service
{
    public class UserService : AppServiceBase
    {
        private readonly UserLocalSerivce _userLocalSerivce;
        private readonly KeyConfigLocalService _keyConfigLocalService;

        public override string ApiServiceUrl => "/api/Admin";
        public UserService(ApiClinetRepository apiClinet, UserLocalSerivce userLocalSerivce, KeyConfigLocalService keyConfigLocalService) : base(apiClinet)
        {
            _userLocalSerivce = userLocalSerivce;
            _keyConfigLocalService = keyConfigLocalService;
        }

        /// <summary>
        /// 获取账号存储信息
        /// </summary>
        /// <returns></returns>
        public async Task<AppliResult<KeyConfig>> GetLocalAccountInfo()
        {
            var r = await _keyConfigLocalService.GetUserSaveInfo();
            return new AppliResult<KeyConfig>() { data = r.data };
        }

        /// <summary>
        /// 获取账号记录
        /// </summary>
        /// <returns></returns>
        public async Task<AppliResult<List<UserInfoDto>>> GetLocalAccountList()
        {
            AppliResult<List<UserInfoDto>> result = new AppliResult<List<UserInfoDto>>();
            var Csql = await _userLocalSerivce.GetListAsync();
            if (Csql.success)
            {
                result.data = new List<UserInfoDto>();
                Csql.data.ForEach(x => result.data.Add(new UserInfoDto() { User = x.Name, Password = x.Password }));
            }
            else
            {
                result.Success = false;
            }

            return result;
        }

        public async Task<AppliResult<string>> SaveLocalAccountInfo(bool Save, UserInfoDto user)
        {
            AppliResult<string> result = new AppliResult<string>();

            var d = await _keyConfigLocalService.GetModelAsync(x => x.Key == AppKeys.SaveUserInfoKey);
            if (d.data!=null)
            {
                d.data.Values = user.User;
                d.data.Check = Save;
                var r = await _keyConfigLocalService.UpdateAsync(d.data);
                if (r.success)
                    result.Message = "保存成功";
            }
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
                    if (info.data == null)
                    {
                        var CSql = await _userLocalSerivce.AddAsync(new SqliteCore.Models.Account() { Name = user.User, Password = user.Password });
                    }
                    _apiClinet.SetToken(api.data.ToString());
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
