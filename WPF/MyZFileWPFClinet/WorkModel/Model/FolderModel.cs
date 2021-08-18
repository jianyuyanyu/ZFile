using System;
using System.Collections.Generic;
using System.Text;

namespace WorkModel
{
   public class FolderModel
    {

        public int Id { get; set; }
        public string Name { get; set; }

        public int Type { get; set; }

        public string Size { get; set; }

        public string format { get; set; }

        public DateTime CRTime { get; set; }

        public string Remark { get; set; }

        public bool IsCheck { get; set; }

    }

    public class DelFile
    {
        public int Id { get; set; }


        public int Type { get; set; }
    }

    public class PasteitemsDto
    {
        public PasteitemsDto()
        {
            Child = new List<PasteitemsChild>();
        }
        public int Pid { get; set; }

        public List<PasteitemsChild> Child { get; set; }

    }
    public class PasteitemsChild
    {
        public int itemId { get; set; }

        public int ItemType { get; set; }
    }

}
