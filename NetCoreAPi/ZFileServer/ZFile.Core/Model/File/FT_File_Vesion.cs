using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZFile.Core.Model.File
{
    public  class FT_File_Vesion
    {
        public int? ComId { get; set; }
        [SqlSugar.SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        public int? RFileID { get; set; }
        public string VesionSM { get; set; }
        public string FileMD5 { get; set; }
        public string Remark { get; set; }
        public string FileSize { get; set; }
        public string CRUser { get; set; }
        public DateTime? CRDate { get; set; }
    }
}
