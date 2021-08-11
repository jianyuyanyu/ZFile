using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZFile.Core.Model.File
{
    public  class FileDocument
    {
        public string Md5 { get; set; }
        public string Qycode { get; set; }
        public string Month { get; set; }
        public string Directory { get; set; }
        public string FileName { get; set; }
        public string FullPath { get; set; }
        public string Extension { get; set; }
        public string ContentType { get; set; }
        public DateTime? RDate { get; set; }
        public DateTime? LDate { get; set; }
        [SqlSugar.SugarColumn(IsPrimaryKey = true, IsIdentity = false,IsNullable =true)]
        public string ID { get; set; }
        public string fileinfo { get; set; }
        public string filesize { get; set; }
        public string ylinfo { get; set; }
        public string isyl { get; set; }
    }
}
