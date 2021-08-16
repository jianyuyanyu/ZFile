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
    public interface IFT_FolderService : IBaseService<FT_Folder>
    {

        public Task<ApiResult<List<FT_FolderDto>>> GetUserCreateFile(int FolderID,string  UserName);
        /// <summary>
        /// 添加文件夹
        /// </summary>
        /// <param name="Folder"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public Task<ApiResult<FT_Folder>> AddFolders(FT_Folder Folder, string UserName);

        
    }
}
