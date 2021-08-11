using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZFile.Core.Model.File
{
    public  class FT_File_UserTag
    {
        public int? ComId { get; set; }
        [SqlSugar.SugarColumn(IsPrimaryKey = true, IsIdentity = false)]
        public int ID { get; set; }
        public int? RefID { get; set; }
        public string TagName { get; set; }
        public string CRUser { get; set; }
        public DateTime? CRDate { get; set; }
    }
}
