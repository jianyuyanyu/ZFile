using System;
using System.Collections.Generic;
using System.Text;

namespace ZFileXamarin.Models
{
    public class Contact
    {
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }

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

    public class TabItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
