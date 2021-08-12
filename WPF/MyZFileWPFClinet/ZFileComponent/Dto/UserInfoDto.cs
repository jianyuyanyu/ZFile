using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component
{

   public class QycodeDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string IsUsed { get; set; }
        public int ID { get; set; }
        public string secret { get; set; }
        public string space { get; set; }
        public DateTime? updatetime { get; set; }
        public DateTime? crdate { get; set; }
        public int filecount { get; set; }
        public string yyspace { get; set; }
    }
   public  class UserInfoDto
    {

        public dynamic Result { get; set; }
        public dynamic Result1 { get; set; }
        public dynamic Result2 { get; set; }
        public dynamic Result3 { get; set; }
        public dynamic Result4 { get; set; }
    }
}
