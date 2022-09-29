using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using ZTAppFramework.PictureMarker.Model;

namespace ZTAppFramework.PictureMarker
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  ZT
    /// 创建时间    ：  2022/9/29 17:02:35 
    /// Description   ：  标记控制
    ///********************************************/
    /// </summary>
    public class MarkCollection
    {
        /// <summary>
        /// 矩阵集合
        /// </summary>
        Dictionary<MarkData, RectangleGeometry> Marks = new Dictionary<MarkData, RectangleGeometry>();
        /// <summary>
        /// 复杂矩阵
        /// </summary>
        PathGeometry Path = new PathGeometry();
        PathGeometry FictPath = new PathGeometry();


        public PathGeometry PathGeometry => RefreshPath();

        public PathGeometry FictPathGemetry => FictRefreshPath();

        public ScaleTransform CurrentScale { get; set; }

        public TranslateTransform CurrentTranslate { get; set; }

        public MarkData this[Point p]
        {
            get => GetMarkAtPoint(p);
            set => Update(value);
        }

        public void Del(MarkData value)
        {
            if (value == null) return;
            MarkData m = Marks.Keys.Where(c => c.Guid == value.Guid).FirstOrDefault();
            if (m == null) return;
            Marks.Remove(m);
            RefreshPath();
        }
        public bool Update(MarkData value)
        {
            MarkData m = Marks.Keys.Where(c => c.Guid == value.Guid).FirstOrDefault();
            if (m == null) return false;
            Marks.Remove(m);
            Add(value);
            RefreshPath();
            return true;
        }

        /// <summary>
        /// 获取离坐标最近的Mark
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public MarkData GetMarkAtPoint(Point p)
        {
            //找到面积最小的一个
            List<MarkData> ms = new List<MarkData>(); MarkData ret = null;
            foreach (MarkData m in Marks.Keys)
            {
                if (Marks[m].FillContains(p))
                {
                    ms.Add(m);
                }
            }
            double min = double.MaxValue;
            ms.All(m =>
            {
                if (m.Visible)
                {
                    double v = Marks[m].GetArea();
                    if (v < min)
                    {
                        min = v;
                        ret = m;
                    }
                }
                return true;
            });
            return ret;
        }

        public void Add(MarkData mark)
        {
            Marks.Add(mark, new RectangleGeometry(new Rect(new Point(0, 0), mark.Rectangle().Size)));
            RectangleGeometry rg = new RectangleGeometry(new Rect(0, 0, mark.Rectangle().Width, mark.Rectangle().Height));
            RotateTransform r = new RotateTransform(mark.Angle);
            TranslateTransform tt = new TranslateTransform(mark.Rectangle().X, mark.Rectangle().Y);
            TransformGroup tg = new TransformGroup();
            tg.Children.Add(r);
            tg.Children.Add(tt);
            Marks[mark].Transform = tg;
            RefreshPath();
        }

        public void Add(MarkData mark, RectangleGeometry geom)
        {
            Marks.Add(mark, geom);
            RefreshPath();
        }
        /// <summary>
        /// 刷新Path
        /// </summary>
        PathGeometry RefreshPath()
        {
            Path.Clear();
            foreach (var item in Marks.Keys)
            {
                if (item.Visible)
                    Path.AddGeometry(Marks[item]);
            }
            return Path;
        }

        PathGeometry FictRefreshPath()
        {
            FictPath.Clear();
            foreach (var item in Marks.Keys)
            {
                if (item.Visible)
                    FictPath.AddGeometry(Marks[item]);
            }
            return FictPath;
        }

        /// <summary>
        /// 根据GUId 找对应举证
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public RectangleGeometry GetRectByMark(MarkData m)
        {
            if (m == null) return null;

            foreach (MarkData mk in Marks.Keys)
            {
                if (mk.Guid == m.Guid)
                {
                    return Marks[mk];
                }
            }
            return null;
        }

        public void CloseAll()
        {
            Marks.Clear();
            Path.Clear();
        }
    }
}
