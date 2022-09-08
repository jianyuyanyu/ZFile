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
using ZTAppFramewrok.Application.Stared;
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
        public async Task<AppliResult<List<LoginParam>>> GetLocalAccountList()
        {
            AppliResult<List<LoginParam>> result = new AppliResult<List<LoginParam>>();
            var Csql = await _userLocalSerivce.GetListAsync();
            if (Csql.success)
            {
                result.data = new List<LoginParam>();
                Csql.data.ForEach(x => result.data.Add(new LoginParam() { Account = x.Name, Password = x.Password }));
            }
            else
            {
                result.Success = false;
            }

            return result;
        }

        public async Task<AppliResult<string>> SaveLocalAccountInfo(bool Save, LoginParam user)
        {
            AppliResult<string> result = new AppliResult<string>();

            var d = await _keyConfigLocalService.GetModelAsync(x => x.Key == AppKeys.SaveUserInfoKey);
            if (d.data!=null)
            {
                d.data.Values = user.Account;
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
        public async Task<AppliResult<string>> LoginServer(LoginParam user)
        {
            AppliResult<string> res = new AppliResult<string>() { Success = false, Message = "未知异常" };
            ApiResult<object> api = await _apiClinet.PostAnonymousAsync<object>(GetEndpoint(), user);
            if (api.success)
            {
                if (api.statusCode == 200)
                {
                    res.Success = true;
                    var info = await _userLocalSerivce.GetModelAsync(x => x.Name == user.Account);
                    if (info.data == null)
                    {
                        var CSql = await _userLocalSerivce.AddAsync(new SqliteCore.Models.Account() { Name = user.Account, Password = user.Password });
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
