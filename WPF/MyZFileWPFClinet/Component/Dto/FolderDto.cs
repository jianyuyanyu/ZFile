using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component
{
    public class FolderData
    {
        public List<FT_FolderDto> FolderInfo { get; set; }

        public List<FT_FileDto> FileInfo { get; set; }
    }

    public class FT_FolderDto
    {
        public int? ComId { get; set; }
        public int ID { get; set; }
        public string Name { get; set; } = "";
        public string FolderType { get; set; } = "";
        public int? PFolderID { get; set; }
        public int? FolderLev { get; set; }
        public string Remark { get; set; } = "";
        public string FolderSpace { get; set; } = "";
        public string ViewAuthUsers { get; set; } = "";
        public string DownloadAuthUsers { get; set; } = "";
        public string UploadaAuthUsers { get; set; } = "";
        public string CollectUser { get; set; } = "";
        public string FoldUploadUrl { get; set; } = "";
        public string FoldDowmLoadUrl { get; set; } = "";
        public string ShareCode { get; set; } = "";
        public string SharePasd { get; set; } = "";
        public string ShareType { get; set; } = "";
        public DateTime? ShareDueDate { get; set; } = DateTime.UtcNow;
        public string CRUser { get; set; } = "";
        public DateTime? CRDate { get; set; } = DateTime.UtcNow;

    }
    public class FT_FileDto
    {
        public int? ComId { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public int FolderID { get; set; }
        public string FileExtendName { get; set; }
        public string FileSize { get; set; }
        public int? FileDownloadCishu { get; set; }
        public string FileMD5 { get; set; }
        public int? FileVersin { get; set; }
        public string IsRecycle { get; set; }
        public string FileUrl { get; set; }
        public string ViewAuthUsers { get; set; }
        public string DownloadAuthUsers { get; set; }
        public string CollectUser { get; set; }
        public string ShareCode { get; set; }
        public string SharePasd { get; set; }
        public string ShareType { get; set; }
        public DateTime? ShareDueDate { get; set; }
        public string ISYL { get; set; }
        public string Remark { get; set; }
        public string UPUser { get; set; }
        public DateTime? UPDDate { get; set; }
        public string CRUser { get; set; }
        public DateTime? CRDate { get; set; }
        public string YLUrl { get; set; }
        public string YLCode { get; set; }
        public string YLPath { get; set; }
        public string YLCount { get; set; }
        public string TBStatus { get; set; }
        public int? TotalTime { get; set; }
        public string zyid { get; set; }
    }

}
