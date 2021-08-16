using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using ZFile.Common;
using ZFile.Common.ApiClient;
using ZFile.Common.EnumHelper;
using ZFile.Core.Model.File;
using ZFile.Extensions.JWT;
using ZFile.Service.DtoModel;
using ZFile.Service.Interfaces;
using ZFile.Service.Repository;

namespace ZFile.Service.Implements
{
    public class FT_FileService : BaseService<FT_File>, IFT_FileService
    {
   

        public async Task<ApiResult<List<FT_FileDtoTwo>>> GetFileListInfo(string foldertype, string FileExtendName, string UserName)
        {
            var res = new ApiResult<List<FT_FileDtoTwo>>
            {
                statusCode = (int)ApiEnum.Error
            };
            if (foldertype == "2")
            {
                res.data = await Db.Queryable<FT_File, FT_Folder>((o, i) => new object[] {
                    JoinType.Left,o.FolderID==i.ID

                }).Where((o, i) =>
                i.FolderType == foldertype &&
                o.CRUser == UserName &&
                (o.Name.StartsWith(FileExtendName)
                ||
                o.FileExtendName
                .StartsWith(FileExtendName)
                )
                ).Select<FT_FileDtoTwo>().ToListAsync();
                res.statusCode = (int)ApiEnum.Status;
            }
            else
            {
                res.data = await Db.Queryable<FT_File, FT_Folder>((o, i) => new object[] {
                    JoinType.Left,o.FolderID==i.ID

                }).Where((o, i) =>
                i.FolderType == foldertype &&
                (o.Name.StartsWith(FileExtendName)
                ||
                o.FileExtendName
                .StartsWith(FileExtendName)
                )
                ).Select<FT_FileDtoTwo>().ToListAsync();
                res.statusCode = (int)ApiEnum.Status;
            }



            return res;
        }
        /// <summary>
        ///  循环查询文件
        /// </summary>
        /// <param name="FolderID"></param>
        /// <param name="ListAll"></param>
        /// <param name="ListID"></param>
        /// <returns></returns>
        public  List<FoldFile> GetNextFloder(int FolderID, List<FT_Folder> ListAll, ref List<FoldFileItem> ListID)
        {
            List<FoldFile> ListData = new List<FoldFile>();
            var list = ListAll.Where(d => d.PFolderID == FolderID);
            foreach (var item in list)
            {
                FoldFile FolderNew = new FoldFile();
                FolderNew.FolderID = item.ID;
                FolderNew.Name = item.Name;
                FolderNew.CRUser = item.CRUser;
                FolderNew.PFolderID = item.PFolderID.Value;
                var SubFileSres =  GetListAsync(o => o.FolderID == item.ID, s => s.ID, DbOrderEnum.Asc).Result;
                FolderNew.SubFileS = SubFileSres.data;
                foreach (var SubFile in FolderNew.SubFileS)
                    ListID.Add(new FoldFileItem() { ID = SubFile.ID, Type = "file" });
                var SubFolderRes =   GetNextFloder(item.ID, ListAll, ref ListID);
                FolderNew.SubFolder = SubFolderRes;
                ListData.Add(FolderNew);
                ListID.Add(new FoldFileItem() { ID = item.ID, Type = "folder" });
            }
            return ListData;
        }


        /// <summary>
        /// 获取权限对应文件路劲
        /// </summary>
        /// <param name="RefType"></param>
        /// <param name="ComId"></param>
        /// <param name="FolderID"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public async Task<ApiResult<List<FT_FileDto>>> GetUserAuthFileGetUserAuthFile(string RefType, int ComId, int FolderID, string UserName)
        {
            var res = new ApiResult<List<FT_FileDto>>
            {
                statusCode = (int)ApiEnum.Error
            };
            var List = await Db.Queryable<FT_File, FT_File_UserAuth>((o, i) => new object[] {
              JoinType.Left,o.ID==i.RefID
            }).Where((o, i) => o.ComId == ComId && o.FolderID == FolderID && o.CRUser == UserName).Select<FT_FileDto>().ToListAsync();
            if (List != null)
            {
                res.statusCode = (int)ApiEnum.Status;
                res.data = List;
            }

            return res;
        }
        public async Task<ApiResult<object>> UploadFile(UploadDto dto)
        {
            var ApiRes = new ApiResult<object> { statusCode = (int)ApiEnum.Error };
            try
            {

                if (string.IsNullOrEmpty(dto.spacecode)) return ApiRes;
                if (dto.chunks > 1)
                {
                    var md5 = await PushChunk(dto.fileMd5, dto.chunk, dto.name, Path.GetExtension(dto.name), dto.Filetype,new MemoryStream(dto.upinfo) );
                    ApiRes.data = md5;
                    ApiRes.statusCode = (int)ApiEnum.Status;
                    return ApiRes;
                }
                else
                {
                    return ApiRes;
                }
            }
            catch (Exception)
            {

                return ApiRes;
            }
        }



        private async Task<string> PushChunk(string wholeMd5, int chunk, string name, string extension, object contentType, Stream stream)
        {
            string rootPath = "";
            var buffer = new byte[stream.Length];
            int c = stream.Read(buffer, 0, buffer.Length);
            var md5 = MD5Util.GetMd5(buffer);
            var realDate = DateTime.Now;
            var filePath = string.Format("{0}\\{1}\\{2:D10}.{3}", rootPath, wholeMd5, chunk, md5);
            await Task.Delay(1);
            SaveDisk(filePath, stream);
            return md5;
        }

        private bool SaveDisk(string filePath, Stream stream)
        {
            string rootPath2 = AppDomain.CurrentDomain.BaseDirectory;
            filePath = rootPath2 + filePath;   //2017-07-01上传文件
            var dir = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                stream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(fileStream);
                fileStream.Close();
            }

            return true;
        }
    }
}
