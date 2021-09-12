using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZFile.Core.Model.File
{
    public class FT_Folder
    {

        public int? ComId { get; set; }
        [SqlSugar.SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
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
}
