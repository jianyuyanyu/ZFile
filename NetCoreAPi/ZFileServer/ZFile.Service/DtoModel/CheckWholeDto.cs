using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZFile.Service.DtoModel
{
   public class CheckWholeDto
    {
        public CheckWholeDto()
        {
            chunkMd5s = new List<string>();
        }
        public List<string> chunkMd5s { get; set; }
    }

    public class DownSplitDto
    {
        public byte[] data { get; set; }
        public int Index { get; set; }
    }
}

