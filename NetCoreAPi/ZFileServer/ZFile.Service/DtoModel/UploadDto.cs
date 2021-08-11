using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZFile.Service.DtoModel
{
    public class UploadDto
    {
        public string fileMd5 { get; set; }

        public long size { get; set; }
        public int chunks { get; set; }
        public int chunk { get; set; }
        public string spacecode { get; set; }

        public byte[] upinfo { get; set; }

        public string name { get; set; }

        public string Filetype { get; set; }
    }
}
