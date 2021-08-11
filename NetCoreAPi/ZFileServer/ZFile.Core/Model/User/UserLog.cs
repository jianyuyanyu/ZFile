using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZFile.Core.Model.User
{
    /// <summary>
    /// 用户日志
    /// </summary>
   public class UserLog
    {
        [SqlSugar.SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        public string loginfo { get; set; }
        public string ip { get; set; }
        public string useraction { get; set; }
        public string remark { get; set; }
    }
}
