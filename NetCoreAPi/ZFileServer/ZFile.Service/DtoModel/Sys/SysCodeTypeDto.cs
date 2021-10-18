using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZFile.Service.DtoModel.Sys
{
    public class SysCodeTypeDto
    {
        public string guid { get; set; }
        public string name { get; set; }
        public string parent { get; set; }
        public int sort { get; set; }
    }

    public class SysCodeTypeTree
    {
        public string id { get; set; }
        public string title { get; set; }
        public List<SysCodeTypeTree> children { get; set; }
        public bool spread { get; set; } = true;
    }
}
