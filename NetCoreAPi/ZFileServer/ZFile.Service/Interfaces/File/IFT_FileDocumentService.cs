using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZFile.Common.ApiClient;
using ZFile.Core.Model.File;
using ZFile.Service.DtoModel;
using ZFile.Service.Repository;

namespace ZFile.Service.Interfaces
{
    public interface IFT_FileDocumentService: IBaseService<FileDocument>
    {

        public Task<bool> Exist(string qyCode, string md5);
        public Task<ApiResult<CheckWholeDto>> Checkwholefile(string spacecode, string md5Client);

        public Task<ApiResult<Object>> UploadFileMerge(UploadDto dto);
    }
}
