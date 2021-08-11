using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZFile.Core.Model.File
{
   public class Qycode
    {

        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string IsUsed { get; set; }
        [SqlSugar.SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        public string secret { get; set; }
        public string space { get; set; }
        public DateTime? updatetime { get; set; }
        public DateTime? crdate { get; set; }
        public int filecount { get; set; }
        public string yyspace { get; set; }
    }
}
