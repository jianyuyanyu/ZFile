using DryIoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using ZTAppFrameword.Template.Control;
using ZTAppFramework.PictureMarker.Extensions;
using ZTAppFramework.PictureMarker.Formulas;
using ZTAppFramework.PictureMarker.Model;

namespace ZTAppFramework.PictureMarker
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  ZT
    /// 创建时间    ：  2022/9/29 11:38:01 
    /// Description   ：  
    ///********************************************/
    /// </summary>
    public class PictureBase
    {
        public Canvas MyCanvas { get; set; }
        public Image ImageControl { get; set; }
        public PictureInfo ImgInfo { get; set; }
        public bool IsDrap { get; set; }//是否拖拽

        public bool IsDrawing = false;//是否绘制

        public bool IsLeftShift = false;

        public bool isEditing = false;//是否修改

        public bool IsCreate = false;//是否创建数据

        protected Path DrawPath = new Path();

        #region 缩放拖拽控件
        public TransformGroup tfGroup = new TransformGroup();

        public ScaleTransform currentScale = new ScaleTransform();

        public TranslateTransform currentTranslate = new TranslateTransform();
        #endregion

        #region 屏幕坐标数据
        protected Point mouseUpPoint;
        protected Point mouseDownPoint;
        protected Point mouseMovePoint;
        public MouseButton MouseButtonState;
        protected Point lastPoint = new Point(0, 0);
        #endregion

        #region 边框数据
        protected DoubleAnimation ThicknessAnima;

        protected RectangleGeometry DrawRec;
        protected EllipseGeometry DrawElliop;
        protected LineGeometry LineGe;
        #endregion

        #region 放大缩小
        public double DefaultScaleLevel = -1;
        public double ScaleLevel = 1;
        public double MaxScaleLeve = 0;
        public double MinScaleLeve = 0;
        #endregion
        public TextBlock PosinText = new TextBlock();
        //相对图片当前坐标
        public Point CurrentPoint { get => relativePoint(Mouse.GetPosition(MyCanvas)); }

        #region Helepr

        CircleHeleper CircleHelepr;
        #endregion
        public PictureBase(Canvas canvas)
        {
            CircleHelepr = new CircleHeleper();
            MyCanvas = canvas;
            MyCanvas.Cursor = Cursors.Cross;
            //滚轮
            MyCanvas.MouseWheel += MyCanvas_MouseWheel;
            //鼠标按下
            MyCanvas.MouseDown += MyCanvas_MouseDown;
            //鼠标抬起
            MyCanvas.MouseUp += MyCanvas_MouseUp;
            //鼠标移动
            MyCanvas.MouseMove += MyCanvas_MouseMove;
            //失去焦点时
            MyCanvas.LostFocus += MyCanvas_LostFocus;
            MyCanvas.MouseLeave += MyCanvas_MouseLeave;
            MyCanvas.MouseEnter += MyCanvas_MouseEnter;
            InitData();
        }

        public void LoadImgFile(string FilePath)
        {
            var data = System.IO.File.ReadAllBytes(FilePath);
            var size = data.GetJpgSize();
            if (size.HasValue)
                LoadImage(data.ImgDataTranformBit(true, 200), size.Value.Width, size.Value.Height);
            else
                LoadImage(data.ImgDataTranformBit());
            FitWindow(false);
        }

        void LoadImage(BitmapImage bitmap, int width = -1, int height = -1)
        {
            ImgInfo.CurrentBitmap = bitmap;
            if (ImageControl != null)
                MyCanvas.Children.Remove(ImageControl);

            IsDrap = IsDrawing = isEditing = false;

            if (width > -1 && height > -1)
            {
                ImgInfo.PictureWidth = width;
                ImgInfo.PictureHeight = height;
                loadAsync(bitmap);
            }
            else
            {
                ImgInfo.PictureWidth = bitmap.PixelWidth;
                ImgInfo.PictureHeight = bitmap.PixelHeight;
            }

            ImageControl = new Image
            {
                Source = bitmap,
                Width = ImgInfo.PictureWidth,
                Height = ImgInfo.PictureHeight,
                Stretch = Stretch.Uniform
            };
            MyCanvas.Children.Add(ImageControl);
            if (DefaultScaleLevel < 0)
            {
                double hr = MyCanvas.ActualWidth / ImgInfo.PictureWidth;
                double vr = MyCanvas.ActualHeight / ImgInfo.PictureHeight;
                ScaleLevel = Math.Max(hr, vr);
            }
            else
            {
                ScaleLevel = DefaultScaleLevel;
            }

            Canvas.SetZIndex(ImageControl, -1);
            resetPositions();
            reRender();
            MyCanvas.Focus();
            ImageControl.Focusable = true;
            ImageControl.Focus();
        }
        async void loadAsync(BitmapImage image)
        {
            BitmapImage bi = null;
            await Task.Run(() =>
            {
                if (bi == null)
                {
                    bi = new BitmapImage();
                    bi.BeginInit();
                    System.IO.Stream source = image.StreamSource;
                    source.Seek(0, System.IO.SeekOrigin.Begin);
                    bi.StreamSource = source;
                    bi.CacheOption = BitmapCacheOption.OnLoad;
                    bi.EndInit();
                    bi.Freeze();
                }
            });

            if (image == ImgInfo.CurrentBitmap)
            {
                ImgInfo.CurrentBitmap = bi;
                ImageControl.Source = bi;
            }
        }

        //放大居住
        public virtual void FitWindow(bool withMaxScale)
        {
            if (ImgInfo.CurrentBitmap == null || MyCanvas == null) return;
            double hr = MyCanvas.ActualWidth / ImgInfo.PictureWidth;
            double vr = MyCanvas.ActualHeight / ImgInfo.PictureHeight;

            if (withMaxScale)
                ScaleLevel = Math.Max(hr, vr);
            else
                ScaleLevel = Math.Min(hr, vr);

            if (withMaxScale == false)
            {
                var scaleWidth = ImgInfo.PictureWidth * ScaleLevel;
                var scaleHeight = ImgInfo.PictureHeight * ScaleLevel;
                currentTranslate.X = (MyCanvas.ActualWidth - scaleWidth) / 2;
                currentTranslate.Y = (MyCanvas.ActualHeight - scaleHeight) / 2;
            }
            else
            {
                currentTranslate.X = currentTranslate.Y = 0;
            }

            reRender();
        }

        #region MouseEvent
        void MyCanvas_MouseEnter(object sender, MouseEventArgs e)
        {
            MyCanvas.Focus();
        }

        void MyCanvas_MouseLeave(object sender, MouseEventArgs e)
        {
            MyCanvas.Focusable = false;
            IsDrap = false;
            IsDrawing = false;
            IsCreate = false;
        }

        void MyCanvas_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        void MyCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (isEditing) return;
            mouseMovePoint = e.GetPosition(MyCanvas);
            MouseMove();
            if (IsDrawing)
            {
                if (!MyCanvas.Children.Contains(DrawPath))
                    MyCanvas.Children.Add(DrawPath);

                MyCanvas.Cursor = Cursors.Cross;
                DrawPath.StrokeThickness = 2 / ScaleLevel;
                Point sP = relativePoint(mouseDownPoint);
                Point eP = getInImagePoint(mouseMovePoint);
                DrawRec.Rect = new Rect(sP, eP);
            }
            else if (IsDrap)
            {
                MyCanvas.Cursor = Cursors.ScrollAll;
                currentTranslate.X += (mouseMovePoint.X - lastPoint.X);
                currentTranslate.Y += (mouseMovePoint.Y - lastPoint.Y);
                e.Handled = true;
                reRender();
            }
            lastPoint = mouseMovePoint;
        }

        protected virtual void MouseMove()
        {
            UpdateTextBox();
        }

        void MyCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (ImageControl == null) return;
            MyCanvas.Cursor = Cursors.Cross;
            mouseUpPoint = e.GetPosition(MyCanvas);
            MouserUp();
            MyCanvas.Focus();
            ImageControl.Focusable = true;
            ImageControl.Focus();
            IsDrap = IsDrawing = isEditing = false;
        }

        protected virtual void MouserUp()
        {
            if (IsDrawing)
            {
                //不允许从图片区域以外开始画线，mouseDownPoint必须在图片区域内
                Point mp = getInImagePoint(mouseUpPoint);
                Rect rect = new Rect(relativePoint(mouseDownPoint), mp);
                if (rect.Width * rect.Height >= 30)
                {
                    Point lp = absolutePoint(new Point(DrawRec.Rect.X, DrawRec.Rect.Y));
                    isEditing = true;
                    Rect range = new Rect(absolutePoint(new Point(0, 0)), new Size(ImgInfo.PictureWidth * ScaleLevel, ImgInfo.PictureHeight * ScaleLevel));
                    //Editor.Show(new Rect(lp.X, lp.Y, DrawRec.Rect.Width * scaleLevel, DrawRec.Rect.Height * scaleLevel), DrawPath.Stroke, scaleLevel, true, range: range);
                    //Editor.Commit();
                    isEditing = false;
                }
                else
                {
                    DrawPath.Visibility = Visibility.Collapsed;
                    // DrawPathList.Visibility = Visibility.Visible;
                    // FictDrawPathList.Visibility = Visibility.Visible;
                }
            }

            UpdateKeyUp();
        }


        void MyCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mouseDownPoint = e.GetPosition(MyCanvas);
            MouseButtonState = e.ChangedButton;
            UpdataKeyDown();
            MouseDown();
            lastPoint = mouseDownPoint;
            MyCanvas.Cursor = Cursors.ScrollAll;

        }

        protected virtual void MouseDown()
        {

            if (IsLeftShift)
            {
                ///显示绘制线隐藏集合线
                Point p = relativePoint(mouseDownPoint);
                IsCreate = true;
                IsDrawing = true;
                DrawPath.Visibility = Visibility.Visible;
                DrawPath.Stroke = Brushes.Red;
                DrawPath.BeginAnimation(Path.StrokeThicknessProperty, ThicknessAnima);
                DrawRec = new RectangleGeometry
                {
                    Rect = new Rect(p.X, p.Y, 0, 0)
                };
                DrawPath.Data = DrawRec;
                IsDrawing = true;
            }
            else
            {
                IsDrap = true;
            }
        }

        void MyCanvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (!MouseWhell()) return;
            double bef = ScaleLevel;
            Point scaleCenter = e.GetPosition((Canvas)sender);
            double scaleTimes = 1.2;
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
                scaleTimes = 1.5;

            if (e.Delta > 0)
                ScaleLevel *= scaleTimes;
            else
                ScaleLevel /= scaleTimes;

            currentScale.CenterX = 0;// relativePoint( scaleCenter).X * scaleLevel;
            currentScale.CenterY = 0;// relativePoint( scaleCenter).Y * scaleLevel;

            double correction = 0;
            if (bef > ScaleLevel)
                correction = 1 / scaleTimes;
            else
                correction = scaleTimes;


            currentTranslate.X += Math.Round(relativePoint(scaleCenter).X, 0) * (bef - ScaleLevel) * correction;
            currentTranslate.Y += Math.Round(relativePoint(scaleCenter).Y, 0) * (bef - ScaleLevel) * correction;
            reRender();
        }

        protected virtual bool MouseWhell()
        {
            if (IsDrawing) return false;
            if (isEditing) return false;
            return true;
        }

        #endregion

        /// <summary>
        /// 若点在图片以外，返回图片内边界点的相对坐标
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        protected Point getInImagePoint(Point p)
        {
            if (ImgInfo.CurrentBitmap == null) return p;
            Point rp = relativePoint(p);
            if (rp.X >= 0 && rp.Y >= 0 && rp.X <= ImgInfo.PictureWidth && rp.Y <= ImgInfo.PictureHeight)
            {
                return rp;
            }

            if (rp.X < 0)
            {
                rp.X = 0;
            }

            if (rp.Y < 0)
            {
                rp.Y = 0;
            }

            if (rp.X > ImgInfo.PictureWidth)
            {
                rp.X = ImgInfo.PictureWidth;
            }

            if (rp.Y > ImgInfo.PictureHeight)
            {
                rp.Y = ImgInfo.PictureHeight;
            }
            return rp;
        }

        public Point absolutePoint(Point r)
        {
            Point sp = currentScale.Transform(new Point(0, 0));
            Point tp = currentTranslate.Transform(new Point(0, 0));
            return new Point(r.X * ScaleLevel + sp.X + tp.X, r.Y * ScaleLevel + sp.Y + tp.Y);
        }
        public Point relativePoint(Point p)
        {
            Point sp = currentScale.Transform(new Point(0, 0));
            Point tp = currentTranslate.Transform(new Point(0, 0));
            return new Point((p.X - sp.X - tp.X) / ScaleLevel, (p.Y - sp.Y - tp.Y) / ScaleLevel);
        }

        /// <summary>
        /// 刷新坐标
        /// </summary>
        private void resetPositions()
        {
            currentScale.CenterX = 0;
            currentScale.CenterY = 0;
            currentTranslate.X = 0;
            currentTranslate.Y = 0;
        }


        public bool IsPointInImage(Point p)
        {
            if (ImgInfo.CurrentBitmap != null)
            {
                Point rp = relativePoint(p);
                if (rp.X >= 0 && rp.Y >= 0 && rp.X <= ImgInfo.PictureWidth && rp.Y <= ImgInfo.PictureHeight)
                {
                    return true;
                }
            }
            return false;
        }

        void UpdateTextBox()
        {
            if (IsPointInImage(mouseMovePoint))
                PosinText.Text = $"X:{Convert.ToInt32(CurrentPoint.X)},Y:{Convert.ToInt32(CurrentPoint.Y)}";
            else
                PosinText.Text = $"已经超出图片坐标X:{Convert.ToInt32(mouseMovePoint.X)}, Y:{Convert.ToInt32(mouseMovePoint.Y)}";
            PosinText.Background = new SolidColorBrush(Colors.Red);
            Canvas.SetLeft(PosinText, mouseMovePoint.X);
            Canvas.SetTop(PosinText, mouseMovePoint.Y + 5);
        }

        public virtual void reRender()
        {
            currentScale.ScaleX = ScaleLevel;
            currentScale.ScaleY = ScaleLevel;
            DrawPath.StrokeThickness = 2 / ScaleLevel;
            DrawPath.RenderTransform = tfGroup;
            if (ImageControl != null)
                ImageControl.RenderTransform = tfGroup;

        }

        void InitData()
        {
            ImgInfo = new PictureInfo();
            ThicknessAnima = new DoubleAnimation();
            ThicknessAnima.RepeatBehavior = new RepeatBehavior(0);
            currentScale.CenterX = currentScale.CenterY = 0;
            DrawPath.Fill = Brushes.Transparent;
            DrawPath.Stroke = Brushes.Red;
            DrawPath.StrokeThickness = 2;
            tfGroup.Children.Add(currentScale);
            tfGroup.Children.Add(currentTranslate);

        }

        public virtual void UpdataKeyDown()
        {
            if (Keyboard.IsKeyDown(Key.LeftShift) && MouseButtonState == MouseButton.Left) IsLeftShift = true;
        }

        public virtual void UpdateKeyUp()
        {
            if (Keyboard.IsKeyUp(Key.LeftShift)) IsLeftShift = false;
        }

    }
}
