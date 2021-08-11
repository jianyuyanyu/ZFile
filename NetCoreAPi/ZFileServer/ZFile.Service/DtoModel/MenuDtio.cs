using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZFile.Service.DtoModel
{
   public class MenuDtio
    {
        public int Id { get; set; }

        public string ModelName { get; set; }

        public string ModelType { get; set; }

        public string ModelUrl { get; set; }

        public string ModelCode { get; set; }

        public int ComId  { get; set; }

        public int ORDERID { get; set; }

        public int IsSys { get; set; }
        public string WXUrl { get; set; }

        public int IsKJFS { get; set; }

        public string PModelCode { get; set; }

        public string Token { get; set; }

        public List<FunData> FunDatas { get; set; }
    }


    public class FunData
    {
        public int ID { get; set; }

        public int ModelID { get; set; }

        public string PageName { get; set; }

        public string ExtData { get; set; }

        public string PageUrl { get; set; }

        public string FunOrder { get; set; }
        public string PageCode { get; set; }

        public string isiframe { get; set; }
    }
}
