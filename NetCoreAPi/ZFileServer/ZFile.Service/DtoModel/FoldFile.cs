using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZFile.Core.Model.File;

namespace ZFile.Service.DtoModel
{
    public class FoldFile
    {
        public int FolderID { get; set; }
        public string Name { get; set; }
        public string CRUser { get; set; }
        public int PFolderID { get; set; }
        public string Remark { get; set; }
        public List<FoldFile> SubFolder { get; set; }
        public List<FT_File> SubFileS { get; set; }
    }

    public class FoldFileItem
    {
        public int ID { get; set; }
        public string Type { get; set; }

    }
}
