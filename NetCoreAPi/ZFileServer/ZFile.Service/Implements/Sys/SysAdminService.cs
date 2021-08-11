using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZFile.Common.ApiClient;
using ZFile.Common.EnumHelper;
using ZFile.Core.Model.User;
using ZFile.Service.DtoModel;
using ZFile.Service.Interfaces;
using ZFile.Service.Repository;

namespace ZFile.Service.Implements
{
    public class SysAdminService : BaseService<UserInfo>, ISysAdminService
    {

        public async Task<ApiResult<UserInfoDto>> LoginAsync(string User, string pasd)
        {
            var res = new ApiResult<UserInfoDto>
            {
                statusCode = (int)ApiEnum.Unauthorized
            };
            try
            {
              
                var UserInfo = await Db.Queryable<UserInfo>().Where(d => d.username == User && d.pasd == Common.Utils.GetMD5(pasd)).FirstAsync();
                if (UserInfo == null)
                {
                    res.statusCode = (int)ApiEnum.Error;
                    res.message = "账号或密码错误";
                    return res;
                }
                res.statusCode = (int)ApiEnum.Status;
                res.message = "登入成功";
                res.data = new UserInfoDto() { User= UserInfo };
             
            }
            catch (Exception)
            {


            }
            return res;
        }


        /// <summary>
        /// 更新个人空间容量
        /// </summary>
        /// <param name="strUserName"></param>
        /// <param name="FileSize"></param>
        /// <returns></returns>
        public async Task<ApiResult<string>> AddSpace(string strUserName, int FileSize)
        {
            var res = new ApiResult<string>
            {
                statusCode = (int)ApiEnum.Unauthorized
            };
            var qymodel = await GetModelAsync(o => o.username == strUserName);
            if (qymodel != null)
            {
                qymodel.data.Space = qymodel.data.Space + FileSize;
            }

            Db.Updateable<UserInfo>().SetColumns(it => it.Space == int.Parse(qymodel.data.Space.ToString())).Where(it => it.username == strUserName).ExecuteCommand();
            res.statusCode = (int)ApiEnum.Status;
            return res;
        }
    }
}
