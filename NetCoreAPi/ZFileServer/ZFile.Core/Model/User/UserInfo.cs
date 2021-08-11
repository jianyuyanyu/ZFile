using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZFile.Core.Model.User
{
   public class UserInfo
    {
        public string username { get; set; }
        public string pasd { get; set; }
        [SqlSugar.SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }

        public string UserRealName { get; set; }

        public string Role { get; set; }

        public int Space { get; set; }
    }
}
