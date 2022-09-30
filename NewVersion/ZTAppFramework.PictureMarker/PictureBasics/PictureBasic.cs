using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using ZTAppFramework.PictureMarker.Formulas;

namespace ZTAppFramework.PictureMarker
{
    public class PictureBasic
    {
        public Canvas MyCanvas { get; set; }
        PictureMouseCollection MC;
        MouseButton mouseButtonStatus;

        #region 动画
        protected DoubleAnimation ThicknessAnima;
        protected RectangleGeometry DrawRec;
        #endregion
        #region Path
        Path DrawPath = new();
        #endregion

        #region KEY状态
        bool IsLeftShift;
        bool IsDraw;
        #endregion
        public PictureBasic(Canvas canvas)
        {
            MyCanvas = canvas;
            MC = new PictureMouseCollection(canvas);
            MC.AddPath(DrawPath);
            MC.MouseDownAction += MouseDown;
            MC.MouseMoveAction += MouseMove;
            MC.MouseUpAction += MouseUp;
            InitData();
        }

        void InitData()
        {
            ThicknessAnima = new DoubleAnimation();
            ThicknessAnima.RepeatBehavior = new RepeatBehavior(0);
            MC.InitData();
        }

        public void LoadImg(string FilePath) => MC.LoadImgFile(FilePath);

        void MouseMove(Point sPoint, Point MovePoint)
        {
            if (IsDraw)
                DrawRectangle(sPoint, MovePoint);
            Point mp = MC.getInImagePoint(MovePoint);
            Rect rect = new Rect(MC.relativePoint(sPoint), MovePoint);
            MC.UpdateTextBox();

        }
        List<Point> points = new List<Point>();
        void MouseUp(Point sPoint, Point ePoint)
        {
            UpdateKeyUp();
            if (IsDraw)
            {
                //不允许从图片区域以外开始画线，mouseDownPoint必须在图片区域内
                Point p = MC.relativePoint(ePoint);
                points.Add(p);
                if (points.Count() >= 2)
                {
                    GeometryGroup group = new GeometryGroup();
                    var Cp = new Point(points[1].X, points[0].Y);
                    LineGeometry lineA = new LineGeometry(points[0], points[1]);
                    LineGeometry lineB = new LineGeometry(points[1], Cp);
                     LineGeometry lineC = new LineGeometry(Cp, points[0]);
                    group.Children.Add(lineA);
                    group.Children.Add(lineB);
                    group.Children.Add(lineC);
                    AngleFormulas angle = new AngleFormulas(points[0], points[1]);
                    var a = angle.GetCosaAngle();
                    DrawPath.Data = group;
                    MessageBox.Show(a.ToString());
                    points.Clear();
                }

            }
            IsDraw = false;
        }

        void MouseDown(Point Point, MouseButton Status)
        {
            mouseButtonStatus = Status;
            UpdataKeyDown();
            if (IsLeftShift)
            {
                DrawRectangle(Point, new Point(0, 0));
                IsDraw = true;
            }

        }

        void DrawRectangle(Point sPoint, Point ePoint)
        {
            MyCanvas.Cursor = Cursors.Cross;
            Point sp = MC.relativePoint(sPoint);
            Point eP = MC.getInImagePoint(ePoint);
            if (eP.X == 0 && eP.Y == 0)
            {
                DrawPath.Visibility = Visibility.Visible;
                DrawPath.Stroke = Brushes.Red;
                DrawPath.BeginAnimation(Path.StrokeThicknessProperty, ThicknessAnima);
                DrawRec = new RectangleGeometry
                {
                    Rect = new Rect(sp.X, sp.Y, 0, 0)
                };
                DrawPath.Data = DrawRec;
            }
            else
            {
                DrawRec.Rect = new Rect(sp, eP);
            }
            MC.SetIsDrap(false);
        }

        public virtual void UpdataKeyDown()
        {
            if (Keyboard.IsKeyDown(Key.LeftShift) && mouseButtonStatus == MouseButton.Left) IsLeftShift = true;
        }

        public virtual void UpdateKeyUp()
        {
            IsLeftShift = false;
        }

    }
}
