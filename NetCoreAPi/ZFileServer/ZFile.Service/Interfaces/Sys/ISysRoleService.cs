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
    public interface ISysRoleService : IBaseService<SysRole>
    {
        /// <summary>
        /// 获得列表
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<Page<SysRole>>> GetPagesAsync(PageParm parm);

        /// <summary>
        /// 查询列表，并获得权限值状态
        /// </summary>
        /// <param name="key">父级</param>
        /// <param name="adminGuid">用户的唯一编号</param>
        /// <returns></returns>
        Task<ApiResult<Page<SysRoleDto>>> GetPagesToRoleAsync(string key, string adminGuid);


        /// <summary>
        /// 添加一条数据
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<string>> AddAsync(SysRole parm);


        /// <summary>
        /// 修改一条数据
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<string>> ModifyAsync(SysRole parm);
    }
}
