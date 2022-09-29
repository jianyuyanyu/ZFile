using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace ZTAppFramework.PictureMarker
{
    public class PictureBasic
    {
        public Canvas MyCanvas { get; set; }
        PictureMouseCollection mouseCollection;
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
            mouseCollection = new PictureMouseCollection(canvas);
            mouseCollection.AddPath(DrawPath);
            mouseCollection.MouseDownAction += MouseDown;
            mouseCollection.MouseMoveAction += MouseMove;
            mouseCollection.MouseUpAction += MouseUp;
            InitData();
        }

        void InitData()
        {
            ThicknessAnima = new DoubleAnimation();
            ThicknessAnima.RepeatBehavior = new RepeatBehavior(0);
            mouseCollection.InitData();
        }

        public void LoadImg(string FilePath) => mouseCollection.LoadImgFile(FilePath);

        void MouseMove(Point sPoint, Point MovePoint)
        {
            if (IsDraw)
                DrawRectangle(sPoint, MovePoint);
            Point mp = mouseCollection.getInImagePoint(MovePoint);
            Rect rect = new Rect(mouseCollection.relativePoint(sPoint), MovePoint);
        

            if (rect.Width * rect.Height >= 30&& DrawRec!=null)
            {
                Point lp = mouseCollection.absolutePoint(new Point(DrawRec.Rect.X, DrawRec.Rect.Y));
                mouseCollection.UpdateTextBox($"宽{DrawRec.Rect.Width},高{DrawRec.Rect.Height}");
            }
        }

        void MouseUp(Point sPoint, Point ePoint)
        {
            UpdateKeyUp();
            if (IsDraw)
            {
                //不允许从图片区域以外开始画线，mouseDownPoint必须在图片区域内
                Point mp = mouseCollection.getInImagePoint(ePoint);
                Rect rect = new Rect(mouseCollection.relativePoint(sPoint), mp);
                if (rect.Width * rect.Height >= 30)
                {
                    Point lp = mouseCollection.absolutePoint(new Point(DrawRec.Rect.X, DrawRec.Rect.Y));
                    //isEditing = true;
                    //Rect range = new Rect(absolutePoint(new Point(0, 0)), new Size(ImgInfo.PictureWidth * ScaleLevel, ImgInfo.PictureHeight * ScaleLevel));
                    ////Editor.Show(new Rect(lp.X, lp.Y, DrawRec.Rect.Width * scaleLevel, DrawRec.Rect.Height * scaleLevel), DrawPath.Stroke, scaleLevel, true, range: range);
                    ////Editor.Commit();
                    //isEditing = false;
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
            Point sp = mouseCollection.relativePoint(sPoint);
            Point eP = mouseCollection.getInImagePoint(ePoint);
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
            mouseCollection.SetIsDrap(false);
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
