
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ZTAppFramework.PictureMarker.Model
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  ZT
    /// 创建时间    ：  2022/9/29 17:03:37 
    /// Description   ：  
    ///********************************************/
    /// </summary>
    public class MarkData
    {
        public MarkData()
        {
            Guid = Guid.NewGuid();
        }

        public Guid Guid { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// 角度
        /// </summary>
        public double Angle { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        public int Layer { get; set; }


        Rect rect = Rect.Empty;

        public Rect Rectangle()
        {
            if (rect == Rect.Empty)
            {
                RectData r = getData();
                rect = r.Rect;
                Angle = r.Angle;
            }
            return rect;
        }

        private RectData getData()
        {
            RectData r = null;
            try
            {
                r = JsonConvert.DeserializeObject<RectData>(Data);

            }
            catch (Exception)
            {
            }
            if (r == null)
            {
                try
                {
                    Point[] pp = JsonConvert.DeserializeObject<Point[]>(Data);
                    Rect rect = new Rect(pp[0], pp[1]); r = new RectData
                    {
                        Rect = rect,
                        Angle = 0
                    };
                }
                catch { }
            }
            if (r == null)
            {
                try
                {
                    string[] data = Data.Replace("\"", "").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (data.Length == 4)
                    {
                        Rect rect = new Rect(new Point(int.Parse(data[0]), int.Parse(data[1])), new Point(int.Parse(data[2]), int.Parse(data[3])));
                        r = new RectData
                        {
                            Rect = rect,
                            Angle = 0
                        };
                    }
                }
                catch { }
            }
            return r;
        }

        //设置矩阵数据
        public void SetRectangle(Rect rec, double angle)
        {

            rect = rec;
            Angle = angle;
            RectData model = new RectData
            {
                Rect = rect,
                Angle = Angle
            };
            Data = JsonConvert.SerializeObject(model);
        }

        public void RefectRectangle()
        {
            RectData MarkItem = JsonConvert.DeserializeObject<RectData>(Data);
            rect = MarkItem.Rect;
            Angle = MarkItem.Angle;
        }

    }


    public class RectData
    {
        public Rect Rect { get; set; }
        public double Angle { get; set; }
        public Dictionary<string, string> CustomDatas { get; set; }
    }
}
