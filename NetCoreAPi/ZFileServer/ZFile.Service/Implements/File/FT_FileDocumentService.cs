using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using ZFile.Common.ApiClient;
using ZFile.Common.EnumHelper;
using ZFile.Core.Model.File;
using ZFile.Service.DtoModel;
using ZFile.Service.Interfaces;
using ZFile.Service.Repository;

namespace ZFile.Service
{
    public class FT_FileDocumentService : BaseService<FileDocument>, IFT_FileDocumentService
    {

        public async Task<bool> Exist(string qyCode, string md5)
        {
            var value = await GetModelAsync(D => D.Qycode == qyCode && D.Md5 == md5);
            return value.data.ID != null;
        }

        /// <summary>
        /// 合并文件
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<ApiResult<object>> UploadFileMerge(UploadDto dto)
        {
            var ApiRes = new ApiResult<object> { statusCode = (int)ApiEnum.Error };
            string[] TempExtra = dto.name.Split('.');
            string ext = TempExtra[TempExtra.Length - 1];
            if (!ext.StartsWith(".")) ext = "." + ext;
            //开始合并文件
            try
            {
                string rootPath2 = AppDomain.CurrentDomain.BaseDirectory;
                FileDocument model = new FileDocument();
                var chunkPath = string.Format("{0}\\{1}", rootPath2, dto.fileMd5);
                var outputFile = string.Format("{0}\\{1}.{2}", rootPath2, dto.fileMd5, ext);
                if (Directory.Exists(chunkPath))
                {
                    var chunkFiles = Directory.GetFiles(chunkPath);
                    var realDate = DateTime.Now;
                    string filePath;
                    if (string.IsNullOrEmpty(dto.spacecode))
                        filePath = string.Format("{0}\\{1:yyyyMM}\\{2}{3}", "", realDate, dto.fileMd5, ext);
                    else
                        filePath = string.Format("{0}\\{4}\\{1:yyyyMM}\\{2}{3}", "", realDate, dto.fileMd5, ext, dto.spacecode);

                    if (!await Exist(dto.spacecode, dto.fileMd5) && SaveDisk(filePath, chunkFiles))
                    {
                        FileInfo fileInfo = new FileInfo(rootPath2 + filePath);
                        long size = fileInfo.Length;
                        model.Md5 = dto.fileMd5;
                        model.Qycode = dto.spacecode;
                        model.FileName = dto.name;
                        model.Extension = ext;
                        model.ContentType = dto.Filetype;
                        model.Directory = Path.GetDirectoryName(rootPath2);
                        model.Month = realDate.ToString("yyyyMM");
                        model.FullPath = filePath;
                        model.RDate = realDate;
                        model.LDate = realDate;
                        model.ID = Guid.NewGuid().ToString("N");
                        model.fileinfo = "WPF";
                        model.isyl = "0";
                        model.ylinfo = "0";
                        model.filesize = size.ToString();
                        await AddAsync(model);
                    }
                    Directory.Delete(chunkPath, true);
                }
                ApiRes.data = model.ID ?? "";
                ApiRes.statusCode = (int)ApiEnum.Status;
            }
            catch (Exception ex)
            {

                ApiRes.message = ex.Message.ToString();
            }
            return ApiRes;
        }
        public async Task<ApiResult<CheckWholeDto>> Checkwholefile(string spacecode, string md5Client)
        {

            var apiRes = new ApiResult<CheckWholeDto>() { statusCode = (int)ApiEnum.HttpRequestError, data = new CheckWholeDto() };
            if (string.IsNullOrEmpty(spacecode) || string.IsNullOrEmpty(md5Client)) return apiRes;

            var result = await Exist(spacecode, md5Client);
            dynamic re = new System.Dynamic.ExpandoObject();
            if (result)
            {
                var doc = await GetModelAsync(d => d.Qycode == spacecode && d.Md5 == md5Client);
                doc.data.RDate = DateTime.Now;
                doc.data.LDate = DateTime.Now;
                doc.data.ID = Guid.NewGuid().ToString("N");
                doc.data.fileinfo = "";
                await AddAsync(doc.data);
                re.zyid = doc.data.ID;
            }
            apiRes.success = result;
            if (!result)
            {
                //不存在，寻找分片内容。
                string chunkPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, md5Client);
                if (Directory.Exists(chunkPath))
                {
                    apiRes.data.chunkMd5s = Directory.GetFiles(chunkPath).Select(o =>
                    {
                        return Path.GetFileName((string)o).Split('.')[1];
                    }).ToList();
                }
            }
            apiRes.statusCode = (int)ApiEnum.Status;

            return apiRes;
        }

        bool SaveDisk(string filePath, string[] chunkFiles)
        {
            string rootPath2 = AppDomain.CurrentDomain.BaseDirectory;
            filePath = rootPath2 + filePath;   //上传文件
            var dir = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            using (Stream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                foreach (string chunkFile in chunkFiles)
                {
                    using (Stream input = File.OpenRead(chunkFile))
                    {
                        input.CopyTo(stream);
                    }
                }
            }
            return true;
        }
    }
}
