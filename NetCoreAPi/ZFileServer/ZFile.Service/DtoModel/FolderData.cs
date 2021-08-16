    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZFile.Service.DtoModel
{
   public class FolderData
    {
        public IEnumerable<FT_FolderDto> FolderInfo { get; set; }

        public IEnumerable<FT_FileDto> FileInfo { get; set; }
    }
}
