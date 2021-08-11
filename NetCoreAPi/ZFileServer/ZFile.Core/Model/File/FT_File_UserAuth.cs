using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZFile.Core.Model.File
{
    public  class FT_File_UserAuth
    {
        public int? ComId { get; set; }
        [SqlSugar.SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        public string AuthType { get; set; }
        public string AuthUser { get; set; }
        public int? RefID { get; set; }
        public string RefType { get; set; }
        public string CRUser { get; set; }
        public DateTime? CRDate { get; set; }
    }
}
