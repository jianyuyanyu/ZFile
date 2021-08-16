using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZFile.Common.ApiClient;
using ZFile.Common.EnumHelper;
using ZFile.Core.Model.File;
using ZFile.Service.DtoModel;
using ZFile.Service.Interfaces;
using ZFile.Service.Repository;

namespace ZFile.Service.Implements.File
{
    public class FT_FolderService : BaseService<FT_Folder>, IFT_FolderService
    {
      
        public async Task<ApiResult<FT_Folder>> AddFolders(FT_Folder Folder,string UserName)
        {
            var res = new ApiResult<FT_Folder>
            {
                statusCode = (int)ApiEnum.Error
            };

            if (Folder.Name == "")
            {
                Folder.Name = "新建文件夹";
            }
            if (Folder.ID == 0)
            {
                Folder.CRUser = UserName;
                Folder.CRDate = DateTime.Now;
                Folder.ComId = 1;
                Folder.ViewAuthUsers = "0";//默认不在回收站 //new FT_FolderB().Insert(Folder);
                 var addRes = await AddAsync(Folder);
                if (addRes.statusCode != 200)
                {
                    res.statusCode = addRes.statusCode;
                    res.success = addRes.success;
                    res.message = addRes.message;
                    return res;
                }
               
                //更新文件夹路径Code
                Folder.Remark = Folder.Remark + "-" + Folder.ID;
                addRes= await UpdateAsync(Folder);
                if (addRes.statusCode != 200)
                {
                    res.statusCode = addRes.statusCode;
                    res.success = addRes.success;
                    res.message = addRes.message;
                    return res;
                }
                res.statusCode = (int)ApiEnum.Status;
                res.data = Folder;
            }
            else
            {

            }

            return res;
        }
      
        /// <summary>
        /// 查询用户创建文件夹
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public async Task<ApiResult<List<FT_FolderDto>>> GetUserCreateFile(int FolderID, string UserName)
        {
            var res = new ApiResult<List<FT_FolderDto>>
            {
                statusCode = (int)ApiEnum.Error
            };

            var model = await Db.Queryable<FT_Folder, FT_File_UserAuth>((o, i) => new object[]{
                JoinType.Left,
                o.ID==i.RefID
            }).Where((o, i) => o.ComId == 1 & o.PFolderID == FolderID && o.CRUser == UserName).Select<FT_FolderDto>().ToListAsync();

            if (model == null) return res;

            res.statusCode = (int)ApiEnum.Status;
            res.data = model;

            return res;
        }
    }
}
