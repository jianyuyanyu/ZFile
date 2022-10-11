using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZTAppFramework.Card.Card;
using ZTAppFramework.Card.Card.smc304;
using ZTAppFramework.Card.Model;

namespace ZTAppFramework.Card.Service
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  WeiXiaolei
    /// 创建时间    ：  2022/10/10 9:26:53 
    /// Description   ：  
    ///********************************************/
    /// </summary>
    public class CardServer
    {

        public Smc304CardFactory CardFactory = new Smc304CardFactory();

        Smc304Card _Cardinfo;
        public bool Connect()
        {   
            if (_Cardinfo != null)return true;
            _Cardinfo = CardFactory.Connect();
            if (_Cardinfo == null) return false;
            return true;
        }

        public bool IsConnect() => _Cardinfo == null ? false : true;

        public Smc304Card GetCard() => _Cardinfo;
        public AxisParm GetAxisParamInfo() => _Cardinfo.GetAxisParamInfo();
        public SMCDefaultModel GetSMCDefaultParamInfo() => _Cardinfo.GetSMCDefaultParamInfo();

    }
}
