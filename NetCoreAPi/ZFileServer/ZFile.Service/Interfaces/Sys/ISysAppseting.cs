using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZFile.Common.ApiClient;
using ZFile.Core.Model;
using ZFile.Service.DtoModel;
using ZFile.Service.Repository;

namespace ZFile.Service.Interfaces
{
    public interface ISysAppseting : IBaseService<FileAppseting>
    {
        /// <summary>
        /// 查询配置信息
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<string>> GetValueByKey(string strKey, string strDefault = "");
    }
}
