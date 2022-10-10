using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTAppFramework.Card.Card.smc304
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  zt
    /// 创建时间    ：  2022/10/10 8:42:59 
    /// Description   ：  雷赛smc304控制卡工厂
    ///********************************************/
    /// </summary>
    public class Smc304CardFactory
    {
        public Smc304Card Connect()
        {
            Smc304Card DevInfo=new Smc304Card();
            if (!DevInfo.LoadAxisParamInfo()) return null;
            if (!DevInfo.LoadCardConfigParamInfo()) return null;
            DevInfo.ConnectCard();
            return DevInfo;
        }


    }
}
