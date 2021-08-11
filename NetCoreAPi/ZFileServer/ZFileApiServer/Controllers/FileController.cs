using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ZFile.Common;
using ZFile.Common.ApiClient;
using ZFile.Common.ConfigHelper;
using ZFile.Common.EnumHelper;
using ZFile.Core.Model.File;
using ZFile.Core.Model.User;
using ZFile.Extensions.Authorize;
using ZFile.Extensions.JWT;
using ZFile.Service.DtoModel;
using ZFile.Service.Interfaces;

namespace ZFileApiServer.Controllers
{
    [Produces("application/json")]
    [Route("api/File")]
    [JwtAuthorize(Roles = "Admin")]
    public class FileController : ControllerBase
    {
        ILogger<FileController> _logger;
        private readonly IFT_FolderService _sysFileFolder;
        private readonly IFT_FileService _sysFille;
        private readonly IFT_File_VesionService _sysFileVersion;
        private readonly ISysAdminService _sysAdmin;
        private readonly IFT_FileDocumentService _FileDocumentService;
        private readonly IFileQycodeService _fileQycode;
        private readonly IFT_FileAuthService _fT_FileAuth;

        public FileController(ILogger<FileController> logger, IFT_FolderService sysFileFolder, IFT_FileService sysFille, IFT_File_VesionService sysFileVersion, ISysAdminService sysAdmin, IFT_FileDocumentService FileDocumentService, IFileQycodeService fileQycode, IFT_FileAuthService fT_FileAuth)
        {
            _logger = logger;
            _sysFileFolder = sysFileFolder;
            _sysFille = sysFille;
            _sysFileVersion = sysFileVersion;
            _sysAdmin = sysAdmin;
            _FileDocumentService = FileDocumentService;
            _fileQycode = fileQycode;
            _fT_FileAuth = fT_FileAuth;
        }

        /// <summary>
        /// 查询网盘文件信息
        /// </summary>
        /// <param name="FolderId"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/GetFolderInfo")]
        public async Task<IActionResult> GetFileInfoData(int Type, int FolderId = 2)
        {
            var apiRes = new ApiResult<FolderData>() { statusCode = (int)ApiEnum.HttpRequestError };
            string[] accesToken = Request.Headers["Authorization"].ToString().Split(' ');
            TokenModel token = JwtHelper.SerializeJWT(accesToken[1]);
            FolderData data = new FolderData();
            if (Type == 1)
            {
                var FolderModel = await _sysFileFolder.GetUserCreateFile(FolderId, token.UserName);
                data.FolderInfo = FolderModel.data;
                var FileModel = await _sysFille.GetUserAuthFileGetUserAuthFile("1", 1, FolderId, "");
                data.FileInfo = FileModel.data;
                apiRes.data = data;
                apiRes.statusCode = (int)ApiEnum.Status;
            }
            else
            {
                var FolderModel = await _sysFileFolder.GetUserCreateFile(FolderId, token.UserName);
                data.FolderInfo = FolderModel.data;
                var FileModel = await _sysFille.GetUserAuthFileGetUserAuthFile("1", 1, FolderId, token.UserName);
                data.FileInfo = FileModel.data;
                apiRes.data = data;
                apiRes.statusCode = (int)ApiEnum.Status;
            }
            return Ok(apiRes);
        }

        /// <summary>
        /// 判断文件MD5校验证是否存在
        /// </summary>
        /// <param name="md5Client"></param>
        /// <param name="spacecode"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/CheckwholeFile")]
        public async Task<IActionResult> Checkwholefile(string md5Client, string spacecode) => Ok(await _FileDocumentService.Checkwholefile(spacecode, md5Client));
        /// <summary>
        /// 查询用户权限
        /// </summary>
        /// <param name="code"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/Checkauth")]
        public async Task<IActionResult> Checkauth(string code, string secret) => Ok(await _fileQycode.Checkauth(code, secret));

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="FolderType"></param>
        /// <param name="PFolderID"></param>
        /// <param name="FolderLev"></param>
        /// <param name="Remark"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/CreateFolder")]
        public async Task<IActionResult> CreateFolders(string Name, string FolderType, int PFolderID, int FolderLev, string Remark)
        {
            string[] accesToken = Request.Headers["Authorization"].ToString().Split(' ');
            TokenModel token = JwtHelper.SerializeJWT(accesToken[1]);
            FT_Folder Folder = new FT_Folder();
            Folder.Name = Name;
            Folder.FolderType = FolderType;
            Folder.PFolderID = PFolderID;
            Folder.FolderLev = FolderLev;
            Folder.Remark = Remark;
            return Ok(await _sysFileFolder.AddFolders(Folder, token.UserName));
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="delFile"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/DelFile")]
        public async Task<IActionResult> DelFLODER([FromBody] List<DelFile> delFile)
        {
            var Apires = new ApiResult<object>
            {
                statusCode = (int)ApiEnum.Error
            };

            foreach (var item in delFile)
            {
                if (item.Type == 1)
                {//删除文件
                    var FileInfo = await _sysFille.GetModelAsync(o => o.ID == item.Id);
                    if (FileInfo.statusCode != 200)
                    {
                        return Ok(Apires);
                    }
                    var DelModel = await _sysFille.DeleteAsync(d => d.ID == item.Id);
                    if (DelModel.statusCode != 200)
                    {
                        return Ok(Apires);
                    }
                    string strZYID = FileInfo.data.zyid;
                    //物理删除
                    var Model = await _FileDocumentService.GetModelAsync(D => D.ID == strZYID); /*new DocumentB().GetEntity(D => D.ID == ID);*/
                    if (Model.statusCode == (int)ApiEnum.Status)
                    {
                        string strFile = AppDomain.CurrentDomain.BaseDirectory+ Model.data.FullPath;
                        var res = await _FileDocumentService.DeleteAsync(Model.data.ID);
                        if (res.statusCode == 200)
                        {
                            if (System.IO.File.Exists(strFile))
                            {
                                System.IO.File.Delete(strFile);
                            }
                        }

                    }
                    //删除目录
                    var AllfolderInfo = await _sysFileFolder.GetListAsync();
                    var Folder = await _sysFileFolder.GetModelAsync(o => o.ID == item.Id);

                    await _fT_FileAuth.DeleteAsync(d => d.RefID == item.Id && d.RefType == "1");
                    Apires.statusCode = 200;

                }
                else
                {
                    //删除目录  
                    var AllfolderInfo = await _sysFileFolder.GetListAsync();
                    var Folder = await _sysFileFolder.GetModelAsync(o => o.ID == item.Id);
                    List<FT_Folder> fs = new List<FT_Folder>();
                    fs.Add(Folder.data);
                    var list = AllfolderInfo.data.Where(o => o.PFolderID == Folder.data.ID);
                    var FileResdata = await _sysFille.GetListAsync();
                    var fileList = FileResdata.data.Where(o => o.FolderID == Folder.data.ID);
                    
                    await _fT_FileAuth.DeleteAsync(d => d.RefID == item.Id && d.RefType == "0");
                    Apires.statusCode = 200;
                }
            }
            return Ok(Apires);
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/Addupload")]
        public async Task<IActionResult> AddFileUpload([FromBody] UploadDto input) => Ok(await _sysFille.UploadFile(input));

        /// <summary>
        /// 上传最后一个文件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/AddFileMerge")]
        public async Task<IActionResult> fileMerge([FromBody] UploadDto input) => Ok(await _FileDocumentService.UploadFileMerge(input));
        /// <summary>
        /// 上传文件成功
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/AddFilesuccess")]
        public async Task<IActionResult> AddFilesuccess([FromBody] List<UploadSuccessDto> dto)
        {
            var Apires = new ApiResult<List<FT_FileDtoTwo>>
            {
                statusCode = (int)ApiEnum.Error
            };
            try
            {
                //获取权限
                string[] accesToken = Request.Headers["Authorization"].ToString().Split(' ');
                TokenModel token = JwtHelper.SerializeJWT(accesToken[1]);
                //
                List<FT_File> ListData = new List<FT_File>();
                var date = DateTime.Now;
                List<FT_File> ListSameData = new List<FT_File>();//重名文件
                foreach (var item in dto)
                {
                    int index = item.filename.LastIndexOf('.');
                    string filename = item.filename.Substring(0, index);
                    string md5 = item.md5;
                    string zyid = item.zyid;
                    var Model = await _sysFille.GetModelAsync(o => o.Name == filename && o.FileExtendName == item.filename.Substring(0, index).ToLower() && o.FolderID == item.FolderID);
                    if (Model.data.ID == 0)//相同目录下没有重复文件
                    {
                        FT_File newfile = new FT_File();
                        newfile.Name = filename;
                        newfile.FileMD5 = md5.Replace("\"", "").Split(',')[0];
                        newfile.zyid = zyid;
                        newfile.FileSize = item.filesize.ToString();
                        newfile.FileVersin = 0;
                        newfile.CRDate = date;
                        newfile.CRUser = token.UserName;
                        newfile.UPDDate = date;
                        newfile.ComId = 1;
                        newfile.UPUser = token.UserName;
                        newfile.FolderID = item.FolderID;
                        newfile.IsRecycle = "0";//默认不在回收站
                        newfile.FileExtendName = item.filename.ToString().Substring(index + 1).ToLower();
                        if (new List<string>() { "txt", "html", "mp3", "doc", "mp4", "flv", "ogg", "avi", "mov", "rmvb", "mkv", "jpg", "gif", "png", "bmp", "jpeg" }.Contains(newfile.FileExtendName.ToLower()))
                        {
                            newfile.ISYL = "Y";
                        }
                        if (new List<string>() { "pdf", "doc", "docx", "ppt", "pptx", "xls", "xlsx" }.Contains(newfile.FileExtendName.ToLower()))
                        {
                            newfile.ISYL = "Y";
                        }
                        if (new List<string>() { "mp4" }.Contains(newfile.FileExtendName.ToLower()))
                        {

                        }
                        ListData.Add(newfile);
                    }
                    else
                    {
                        FT_File_Vesion Vseion = new FT_File_Vesion();
                        Vseion.RFileID = Model.data.ID;
                        Vseion.FileSize = Model.data.FileSize;
                        Vseion.CRDate = date;
                        Vseion.CRUser = token.UserName;
                        var model = await _sysFileVersion.AddAsync(Vseion);//加入新的版本
                        if (model.statusCode != 200)
                        {
                            Apires.message = model.message;
                            return Ok(Apires);
                        }
                        Model.data.FileVersin = Model.data.FileVersin + 1;
                        Model.data.FileMD5 = md5.Replace("\"", "").Split(',')[0];
                        Model.data.zyid = md5.Split(',').Length == 2 ? md5.Split(',')[1] : md5.Split(',')[0];
                        Model.data.FileSize = item.filesize.ToString();
                        Model.data.UPDDate = date;
                        Model.data.UPUser = token.UserName;
                        model = await _sysFille.UpdateAsync(Model.data); //修改新版本
                        if (model.statusCode != 200)
                        {
                            Apires.message = model.message;
                            return Ok(Apires);
                        }
                        ListSameData.Add(Model.data);
                    }
                }
                foreach (FT_File item in ListData)
                {
                    //添加文件数据
                    await _sysFille.AddAsync(item);
                    int filesize = 0;
                    //更新企业空间占用
                    var User = await _sysAdmin.GetModelAsync(o => o.username == token.UserName);
                    int.TryParse(item.FileSize, out filesize);
                    User.data.Space = User.data.Space + filesize;
                    await _sysAdmin.UpdateAsync(User.data);
                }
                Apires.statusCode = (int)ApiEnum.Status;
            }
            catch (Exception ex)
            {
                Apires.message = "调用上传文件接口出错" + ex.Message.ToString();
            }
            return Ok(Apires);
        }

        /// <summary>
        /// 查询要下载得文件信息
        /// </summary>
        /// <param name="FileId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/GetDownLoadFileInfo")]
        public async Task<IActionResult> GetDownLoadFileInfo(int FileId)
        {
            var Apires = new ApiResult<object>
            {
                statusCode = (int)ApiEnum.Error
            };
            var FileInfo = await _sysFille.GetModelAsync(o => o.ID == FileId);
            if (FileInfo.data.ID != 0)
            {
                var FileMolde = await _FileDocumentService.GetModelAsync(O => O.ID == FileInfo.data.zyid);
                if (FileMolde.data.ID != null)
                {
                    try
                    {
                        if (!System.IO.File.Exists(FileMolde.data.Directory + FileMolde.data.FullPath))
                        {
                            Apires.message = "文件不存在！";
                            return Ok(Apires);
                        }
                        FileStream SplitFileStream = new FileStream(FileMolde.data.Directory + FileMolde.data.FullPath, FileMode.Open);
                        int iFileCount = (int)(int.Parse(FileMolde.data.filesize) / (1000 * 1024));
                        if (SplitFileStream.Length % (1000 * 1024) != 0) iFileCount++;
                        SplitFileStream.Close();
                        var mode = new
                        {
                            FileData = FileMolde.data.ID,
                            FileExt = FileMolde.data.Extension,
                            FileSize = FileMolde.data.filesize,
                            FileCount = iFileCount
                        };
                        Apires.data = mode;
                        Apires.statusCode = (int)ApiEnum.Status;
                    }
                    catch (Exception)
                    {

                    }

                    //Apires.data = 

                }

            }
            return Ok(Apires);
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="Index"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/DownloadFile")]
        public async Task<IActionResult> DownloadFile(string Code, int Index)
        {
            var Apires = new ApiResult<DownSplitDto>
            {
                statusCode = (int)ApiEnum.Error,
                data = new DownSplitDto()
            };
            var Model = await _FileDocumentService.GetModelAsync(O => O.ID == Code);
            if (Model.data.ID != null)
            {
                if (!System.IO.File.Exists(Model.data.Directory + Model.data.FullPath))
                {
                    Apires.message = "文件不存在！";
                    return Ok(Apires);
                }
                byte[] TempBytes;
                int iFileSize = 1000 * 1024;
                //以文件的全路径对应的字符串和文件打开模式来初始化FileStream文件流实例
                using (FileStream SplitFileStream = new FileStream(Model.data.Directory + Model.data.FullPath, FileMode.Open))
                {
                    SplitFileStream.Seek(Index * iFileSize, SeekOrigin.Begin);
                    //以FileStream文件流来初始化BinaryReader文件阅读器
                    using (BinaryReader SplitFileReader = new BinaryReader(SplitFileStream))
                    {

                        TempBytes = SplitFileReader.ReadBytes(iFileSize);
                    }
                }
                Apires.data.data = TempBytes;
                Apires.data.Index = Index;
                Apires.statusCode = (int)ApiEnum.Status;


                //using (var fs = new FileStream(Model.data.Directory + Model.data.FullPath, FileMode.Open))
                //{
                //    int shardSize = 1000 * 1024;
                //    int FileCount = (int)(int.Parse(Model.data.filesize) / shardSize);
                //    if ((int.Parse(Model.data.filesize) % shardSize) > 0)
                //    {
                //        FileCount += 1;
                //    }
                //    if (Index > FileCount - 1)
                //    {
                //        Apires.message = "无效的下标！";
                //        return Ok(Apires);
                //    }
                //    if (Index == FileCount - 1)
                //    {
                //        //最后一片 = 总长 - (每次片段大小 * 已下载片段个数)
                //        shardSize = (int)(int.Parse(Model.data.filesize) - (shardSize * Index));
                //    }
                //    byte[] datas = new byte[shardSize];
                //    await fs.ReadAsync(datas, 0, datas.Length);
                //    Apires.data = datas;
                //    Apires.statusCode = (int)ApiEnum.Status;
                //}
            }
            return Ok(Apires);
        }

    }
}
