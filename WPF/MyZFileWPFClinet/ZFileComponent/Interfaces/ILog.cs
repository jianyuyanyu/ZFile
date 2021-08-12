using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.Interfaces
{
    public interface ILog
    {
        void Debug(string msg);
        void Info(string msg);
        void Error(Exception ex);
        void Error(string msg);
    }
}
