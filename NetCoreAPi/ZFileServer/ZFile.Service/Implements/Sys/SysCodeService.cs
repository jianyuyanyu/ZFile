using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZFile.Core.Model.User;
using ZFile.Service.Repository;
using ZFile.Common.ApiClient;
using ZFile.Common.EnumHelper;
using ZFile.Common.LogHelper;
using SqlSugar;
using ZFile.Service.DtoModel;
using ZFile.Service.Extensions;

namespace ZFile.Service.Implements
{

    /// <summary>
    /// 字典值实现
    /// </summary>
    public class SysCodeService : BaseService<SysCode>
    {

        /// <summary>
        /// 获得一条数据
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<ApiResult<SysCode>> GetByGuidAsync(string parm)
        {
            var model = await Db.Queryable<SysCode>().SingleAsync(m => m.Guid == parm);
            var res = new ApiResult<SysCode>
            {
                statusCode = 200,
                data = model != null ? model : new SysCode() { }
            };
            if (model == null)
            {
                var pmdel = await Db.Queryable<SysCode>().OrderBy(m => m.Sort, OrderByType.Desc).FirstAsync();
                res.data = new SysCode() { Status = true, Sort = pmdel?.Sort + 1 ?? 1 };
            }
            return res;
        }

        /// <summary>
        /// 添加一条记录
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<ApiResult<string>> AddAsync(SysCode parm)
        {
            var res = new ApiResult<string>() { statusCode = 200 };
            try
            {
                //判断是否存在
                var isExt = SysCodeDb.IsAny(m => m.Name == parm.Name && m.ParentGuid == parm.ParentGuid);
                if (isExt)
                {
                    res.statusCode = (int)ApiEnum.ParameterError;
                    res.message = "该名称已存在~";
                }
                else
                {
                    parm.Guid = Guid.NewGuid().ToString();
                    var dbres = await Db.Insertable(parm).ExecuteCommandAsync();
                    if (dbres == 0)
                    {
                        res.statusCode = (int)ApiEnum.Error;
                        res.message = "插入数据失败~";
                    }
                }
            }
            catch (Exception ex)
            {
                res.statusCode = (int)ApiEnum.Error;
                res.message = ApiEnum.Error.GetEnumText() + ex.Message;
                Logger.Default.ProcessError((int)ApiEnum.Error, ex.Message);
            }
            return res;
        }

        /// <summary>
        ///  查询列表，根据条件
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<ApiResult<Page<SysCode>>> GetPagesAsync(SysCodePostPage parm)
        {
            var res = new ApiResult<Page<SysCode>>();
            try
            {
                res.data = await Db.Queryable<SysCode>()
                        .WhereIF(!string.IsNullOrEmpty(parm.guid), m => m.ParentGuid == parm.guid)
                        .OrderBy(m => m.Sort).ToPageAsync(parm.page, parm.limit);
            }
            catch (Exception ex)
            {
                res.message = ApiEnum.Error.GetEnumText() + ex.Message;
                res.statusCode = (int)ApiEnum.Error;
                Logger.Default.ProcessError((int)ApiEnum.Error, ex.Message);
            }
            return res;
        }

        /// <summary>
        /// 修改一条记录
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<ApiResult<string>> ModifyAsync(SysCode parm)
        {
            var isok = await Db.Updateable<SysCode>().SetColumns(
               m => new SysCode()
               {
                   Name = parm.Name,
                   CodeType = parm.CodeType,
                   Summary = parm.Summary,
                   Status = parm.Status,
                   EditTime = DateTime.Now
               }).Where(m => m.Guid == parm.Guid).ExecuteCommandAsync();
            var res = new ApiResult<string>
            {
                success = isok > 0,
                statusCode = isok > 0 ? (int)ApiEnum.Status : (int)ApiEnum.Error,
                data = isok > 0 ? "1" : "0"
            };
            return res;
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<ApiResult<string>> ModifyStatusAsync(SysCode parm)
        {
            var isok = await Db.Updateable<SysCode>().SetColumns(
                m => new SysCode()
                {
                    Status = parm.Status,
                    EditTime = DateTime.Now
                }).Where(m => m.Guid == parm.Guid).ExecuteCommandAsync();
            var res = new ApiResult<string>
            {
                success = isok > 0,
                statusCode = isok > 0 ? (int)ApiEnum.Status : (int)ApiEnum.Error,
                data = isok > 0 ? "1" : "0"
            };
            return res;
        }

    }
}
