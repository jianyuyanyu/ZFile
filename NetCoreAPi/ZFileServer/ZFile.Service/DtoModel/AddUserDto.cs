using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ZFile.Service.DtoModel
{
  public  class AddUserDto
    {
        [DisplayName("账号")]
        public string UserName { get; set; }
        [DisplayName("密码")]
        public string Password { get; set; }
    }
}
