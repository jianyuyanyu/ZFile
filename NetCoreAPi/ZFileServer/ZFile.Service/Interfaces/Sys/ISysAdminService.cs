using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZFile.Common.ApiClient;
using ZFile.Core.Model.User;
using ZFile.Service.DtoModel;
using ZFile.Service.Repository;

namespace ZFile.Service.Interfaces
{
   public interface  ISysAdminService : IBaseService<UserInfo>
    {

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<ApiResult<UserInfoDto>> LoginAsync(string User,string pasd );

        /// <summary>
        /// 更新容量
        /// </summary>
        /// <param name="User"></param>
        /// <param name="pasd"></param>
        /// <returns></returns>
        Task<ApiResult<string>> AddSpace(string strUserName, int FileSize);
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        //public Task<ApiResult<Msg_Result>> GetUserInfo(UserInfoDto user);


    }
}
