using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZFile.Common.ApiClient;
using ZFile.Core.Model.File;
using ZFile.Extensions.JWT;
using ZFile.Service.DtoModel;
using ZFile.Service.Repository;

namespace ZFile.Service.Interfaces
{
   public interface IFT_FileService : IBaseService<FT_File>
    {   
        /// <summary>
        /// 查询用户文件
        /// </summary>
        /// <param name="RefType"></param>
        /// <param name="ComId"></param>
        /// <param name="FolderID"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public Task<ApiResult<List<FT_FileDto>>> GetUserAuthFileGetUserAuthFile(string RefType, int ComId, int FolderID, string UserName);
        /// <summary>
        /// 查询文件
        /// </summary>
        /// <param name="foldertype"></param>
        /// <param name="FileExtendName"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public Task<ApiResult<List<FT_FileDtoTwo>>> GetFileListInfo(string foldertype, string FileExtendName, string UserName);

        public Task<ApiResult<Object>> UploadFile(UploadDto dto);

    }
}
