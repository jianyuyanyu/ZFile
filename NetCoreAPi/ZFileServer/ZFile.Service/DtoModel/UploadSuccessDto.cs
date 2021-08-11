using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZFile.Service.DtoModel
{
   public class UploadSuccessDto
    {
        public string filename { get; set; }
        public string md5 { get; set; }

        public long filesize { get; set; }

        public string zyid { get; set; }

        public int FolderID { get; set; }
    }
}
