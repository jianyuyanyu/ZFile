using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.Interfaces
{
    public interface IMsg
    {
        void Error(string msg);
        void Warning(string msg);
        bool Question(string msg);
    }
}
