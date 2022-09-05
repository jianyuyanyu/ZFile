using NetTaste;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZFile.Service.DtoModel
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  Zt
    /// 创建时间    ：  2022/9/5 17:17:23 
    /// Description   ：  
    ///********************************************/
    /// </summary>
    public class AuthenticateDto
    {
        public string Token  { get; set; }

        public string Role { get; set; }

        public string RefreshToken { get; set; }
        public long TokenExpiration{ get; set; }
    }
}
