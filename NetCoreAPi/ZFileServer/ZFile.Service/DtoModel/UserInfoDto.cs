using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZFile.Core.Model.File;
using ZFile.Core.Model.User;

namespace ZFile.Service.DtoModel
{
   public class UserInfoDto
    {
        public UserInfo User;
        public string UserRoleCode;
        public Qycode Qycode;
        public JH_Auth_QYDto QYinfo;
    }
}
