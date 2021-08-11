using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZFile.Common.ApiClient;
using ZFile.Core.Model.File;
using ZFile.Service.Repository;

namespace ZFile.Service.Interfaces
{
  public interface IFileQycodeService: IBaseService<Qycode>
    {
        public Task<ApiResult<Qycode>> GetALLEntities();
        public Task<ApiResult<string>> Checkauth(string Code, string Secret);
    }
}
