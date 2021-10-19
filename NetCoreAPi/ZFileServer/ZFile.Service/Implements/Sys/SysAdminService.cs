using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZFile.Common;
using ZFile.Common.ApiClient;
using ZFile.Common.EnumHelper;
using ZFile.Core.Model.User;
using ZFile.Service.DtoModel;
using ZFile.Service.Interfaces;
using ZFile.Service.Repository;

namespace ZFile.Service.Implements
{
    public class SysAdminService : BaseService<SysAdmin>, ISysAdminService
    {

        public async Task<ApiResult<UserInfoDto>> LoginAsync(string User, string pasd)
        {
            var res = new ApiResult<UserInfoDto>
            {
                statusCode = (int)ApiEnum.Unauthorized
            };
            try
            {
              
                var UserInfo = await Db.Queryable<SysAdmin>().Where(d => d.LoginName == User && d.LoginPwd == Common.Utils.GetMD5(pasd)).FirstAsync();
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
            catch (Exception ex)
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
            var qymodel = await GetModelAsync(o => o.LoginName == strUserName);
            if (qymodel != null)
            {
                qymodel.data.Space = qymodel.data.Space + FileSize;
            }

            Db.Updateable<SysAdmin>().SetColumns(it => it.Space == int.Parse(qymodel.data.Space.ToString())).Where(it => it.LoginName == strUserName).ExecuteCommand();
            res.statusCode = (int)ApiEnum.Status;
            return res;
        }

        /// <summary>
        /// 根据登入账号，返回菜单信息
        /// </summary>
        /// <param name="Admin"></param>
        /// <returns></returns>
        List<SysMenuDto> GetMenuByAdmin(string admin)
        {
            List<SysMenuDto> res = null;
            try
            {
                //根据用户查询角色列表， 一个用户对应多个角色
                var roleList = SysPermissionsDb.GetList(m => m.AdminGuid == admin && m.Types == 2).Select(m => m.RoleGuid).ToList();
                //根据角色查询菜单，并查询到菜单涉及的功能
                var query = Db.Queryable<SysMenu, SysPermissions>((sm, sp) => new object[]{
                    JoinType.Left,sm.Guid==sp.MenuGuid
                })
                .Where((sm, sp) => roleList.Contains(sp.RoleGuid) && sp.Types == 1 && sm.Status)
                .OrderBy((sm, sp) => sm.Sort)
                .Select((sm, sp) => new SysMenuDto()
                {
                    guid = sm.Guid,
                    parentGuid = sm.ParentGuid,
                    parentName = sm.ParentName,
                    name = sm.Name,
                    nameCode = sm.NameCode,
                    parentGuidList = sm.ParentGuidList,
                    layer = sm.Layer,
                    urls = sm.Urls,
                    icon = sm.Icon,
                    sort = sm.Sort,
                    btnJson = sp.BtnFunJson
                })
                .Mapper((it, cache) => {
                    var codeList = cache.Get(list =>
                    {
                        return Db.Queryable<SysCode>().Where(m => m.ParentGuid == "a88fa4d3-3658-4449-8f4a-7f438964d716")
                            .Select(m => new SysCodeDto()
                            {
                                guid = m.Guid,
                                name = m.Name,
                                codeType = m.CodeType
                            })
                            .ToList();
                    });
                    if (!string.IsNullOrEmpty(it.btnJson))
                    {
                        it.btnFun = codeList.Where(m => it.btnJson.Contains(m.guid)).ToList();
                    }
                });
                var result = query.ToList();
                res = result.CurDistinct(m => m.guid).ToList();
            }
            catch
            {
                res = null;
            }
            return res;
        }


    }
}
