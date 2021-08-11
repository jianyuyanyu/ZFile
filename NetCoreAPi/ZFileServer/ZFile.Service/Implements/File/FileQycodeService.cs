using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZFile.Common.ApiClient;
using ZFile.Common.EnumHelper;
using ZFile.Core.Model.File;
using ZFile.Service.Interfaces;
using ZFile.Service.Repository;

namespace ZFile.Service.Implements
{
    public class FileQycodeService : BaseService<Qycode>, IFileQycodeService
    {
        public async Task<ApiResult<Qycode>> GetALLEntities()
        {
            var res = new ApiResult<Qycode>
            {
                statusCode = (int)ApiEnum.Error
            };
            try
            {
                res.statusCode = (int)ApiEnum.Status;
                res.data = await Db.Queryable<Qycode>().FirstAsync();
                return res;
            }
            catch (Exception)
            {
                return res;
            }
            
        }

        public async Task<ApiResult<string>> Checkauth(string Code, string Secret)
        {
            var apiRes = new ApiResult<string>() { statusCode = (int)ApiEnum.Error, data="N"};

            var res = await GetModelAsync(d => d.Code == Code && d.secret == Secret);
            if (res.data.Code!=null)
            {
                apiRes.statusCode = (int)ApiEnum.Status;
                apiRes.data = "Y";
            }
            else
            {
                apiRes.data = "N";
                apiRes.message = "获取空间失败";
            }

            return apiRes;
        }
    }
}
