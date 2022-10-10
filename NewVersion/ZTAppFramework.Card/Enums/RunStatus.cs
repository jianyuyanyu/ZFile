using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTAppFramework.Card
{
  
    /// <summary>
    /// 控制卡运行状态
    /// </summary>
    public enum RunStatus
    {
        Normal,  //正常
        Run,//运行
        GoHome,//回零
        Machining,//加工
        Suspend,//暂停
        Emstop,//急停
    }
}
