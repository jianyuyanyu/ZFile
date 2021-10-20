using System.Collections.Generic;
using System.Threading.Tasks;
using ZFile.Common.ApiClient;
using ZFile.Core.Model.User;
using ZFile.Service.DtoModel;
using ZFile.Service.Repository;

namespace ZFile.Service.Interfaces
{
    /// <summary>
    /// 角色菜单业务接口
    /// </summary>
    public interface ISysPermissionsService : IBaseService<SysPermissions>
    {
        /// <summary>
        /// 保存角色菜单信息
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<string>> SaveAsync(SysPermissions parm);

        /// <summary>
        /// 用户授权角色
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<string>> ToRoleAsync(SysPermissions parm, bool status);

        /// <summary>
        /// 菜单授权-菜单功能
        /// role=角色
        /// funguid=按钮的编号
        /// status=取消还是授权
        /// </summary>
        /// <returns></returns>
        ApiResult<string> RoleMenuToFunAsync(SysPermissionsParm parm);

        /// <summary>
        /// 保存角色菜单信息
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<List<SysPermissions>>> GetListAsync(string roleGuid);

        /// <summary>
        /// 权限管理-保存角色和授权菜单以及功能
        /// </summary>
        /// <returns></returns>
        ApiResult<string> SaveAuthorization(List<SysMenuDto> list, string roleGuid);
    }
}