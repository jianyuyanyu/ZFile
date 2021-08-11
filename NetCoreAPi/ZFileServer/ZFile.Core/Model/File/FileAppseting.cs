using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZFile.Core.Model
{
   public  class FileAppseting
    {
        [SqlSugar.SugarColumn(IsPrimaryKey = true, IsIdentity = false)]
        public string Key { get; set; }
        public string Value { get; set; }
        public string beizhu { get; set; }
        public string sm { get; set; }
    }
}
