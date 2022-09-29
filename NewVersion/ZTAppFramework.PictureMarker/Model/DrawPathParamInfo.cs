using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace ZTAppFramework.PictureMarker.Model
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  ZT
    /// 创建时间    ：  2022/9/29 17:26:38 
    /// Description   ：  
    ///********************************************/
    /// </summary>
    public class DrawPathParamInfo
    {
        public Dictionary<int, PathInfoModel> MarkPaths =new Dictionary<int, PathInfoModel>();


        public void Add(int Layer,PathInfoModel model)
        {

        }
    }
}
