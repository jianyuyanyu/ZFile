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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ZTAppFramework.PictureMarker.Extensions;

namespace ZTAppFramework.PictureMarker
{
    public class PictureMouseCollection
    {
        public Canvas MyCanvas { get; set; }
        public Image ImageControl { get; set; }
        public PictureInfo ImgInfo { get; set; }


        #region 缩放拖拽控件
        public TransformGroup tfGroup = new TransformGroup();

        public ScaleTransform currentScale = new ScaleTransform();

        public TranslateTransform currentTranslate = new TranslateTransform();
        #endregion

        #region 放大缩小
        public double DefaultScaleLevel = -1;
        public double ScaleLevel = 1;
        #endregion

        #region 屏幕坐标数据
        public Point mouseUpPoint;
        public Point mouseDownPoint;
        public Point mouseMovePoint;
        public MouseButton MouseButtonState;
        protected Point lastPoint = new Point(0, 0);

        #endregion

        #region 绘制数据处理
        List<Path> Paths = new List<Path>();
        #endregion

        #region 图片处理
        public void LoadImgFile(string FilePath)
        {
            var data = System.IO.File.ReadAllBytes(FilePath);
            var size = data.GetJpgSize();
            if (size.HasValue)
                LoadImage(data.ImgDataTranformBit(true, 200), size.Value.Width, size.Value.Height);
            else
                LoadImage(data.ImgDataTranformBit());
        }

        void LoadImage(BitmapImage bitmap, int width = -1, int height = -1)
        {
            ImgInfo.CurrentBitmap = bitmap;
            if (ImageControl != null)
                MyCanvas.Children.Remove(ImageControl);
            SetIsDrap(false);
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
        #endregion
        public PictureMouseCollection(Canvas canvas)
        {
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
        }

        #region 提供属性
        public Action<Point, MouseButton> MouseDownAction;
        public Action<Point, Point> MouseMoveAction;
        public Action<Point, Point> MouseUpAction;
        bool IsDrap = false;
        bool IsWheel = true;
        public void SetIsDrap(bool drap) => IsDrap = drap;
        public void SetIsWheel(bool Wheel) => IsWheel = Wheel;

        public TextBlock PosinText = new TextBlock();
        #endregion

        #region 提供参数

        public void AddPath(Path path)
        {
            path.Fill = Brushes.Transparent;
            path.Stroke = Brushes.Red;
            path.StrokeThickness = 1;
            Paths.Add(path);
        }

        public void CloseAllPath() => Paths.Clear();
        #endregion

        #region 鼠标控制
        void MyCanvas_LostFocus(object sender, RoutedEventArgs e)
        {

        }
        void MyCanvas_MouseEnter(object sender, MouseEventArgs e)
        {
            MyCanvas.Focus();
            ImageControl.Focusable = true;
            ImageControl.Focus();
        }
        void MyCanvas_MouseLeave(object sender, MouseEventArgs e)
        {
            MyCanvas.Focusable = false;
        }
        void MyCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (ImageControl == null) return;
            mouseDownPoint = e.GetPosition(MyCanvas);
            MouseButtonState = e.ChangedButton;
            SetIsDrap(true);
            MouseDownAction?.Invoke(mouseDownPoint, MouseButtonState);
            lastPoint = mouseDownPoint;
            MyCanvas.Cursor = Cursors.ScrollAll;
        }
        void MyCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (ImageControl == null) return;
            mouseMovePoint = e.GetPosition(MyCanvas);
            foreach (var path in Paths)
                if (!MyCanvas.Children.Contains(path))
                    MyCanvas.Children.Add(path);
            MouseMoveAction?.Invoke(mouseDownPoint, mouseMovePoint);
            if (IsDrap)
            {
                MyCanvas.Cursor = Cursors.ScrollAll;
                currentTranslate.X += (mouseMovePoint.X - lastPoint.X);
                currentTranslate.Y += (mouseMovePoint.Y - lastPoint.Y);
                e.Handled = true;
                reRender();
            }
            lastPoint = mouseMovePoint;
        }
        void MyCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (ImageControl == null) return;
            MyCanvas.Cursor = Cursors.Cross;
            mouseUpPoint = e.GetPosition(MyCanvas);
            MouseUpAction?.Invoke(mouseDownPoint, mouseUpPoint);
            IsDrap = false;
        }
        void MyCanvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (ImageControl == null) return;
            if (!IsWheel) return;
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

        #endregion

        #region 坐标转换
        public Point getInImagePoint(Point p)
        {
            if (ImageControl == null) return p;
            Point rp = relativePoint(p);
            if (rp.X >= 0 && rp.Y >= 0 && rp.X <= ImgInfo.PictureWidth && rp.Y <= ImgInfo.PictureHeight) return rp;
            if (rp.X < 0) rp.X = 0;
            if (rp.Y < 0) rp.Y = 0;
            if (rp.X > ImgInfo.PictureWidth) rp.X = ImgInfo.PictureWidth;
            if (rp.Y > ImgInfo.PictureHeight) rp.Y = ImgInfo.PictureHeight;
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

        #endregion

        #region 数据处理
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

        public virtual void reRender()
        {
            currentScale.ScaleX = ScaleLevel;
            currentScale.ScaleY = ScaleLevel;
            foreach (var Pathitem in Paths)
            {
                Pathitem.StrokeThickness = 2 / ScaleLevel;
                Pathitem.RenderTransform = tfGroup;
            }
            if (ImageControl != null)
                ImageControl.RenderTransform = tfGroup;
        }

        public void InitData()
        {
            ImgInfo = new PictureInfo();
            currentScale.CenterX = currentScale.CenterY = 0;
            tfGroup.Children.Add(currentScale);
            tfGroup.Children.Add(currentTranslate);
            MyCanvas.Children.Add(PosinText);
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

        public Point CurrentPoint { get => relativePoint(Mouse.GetPosition(MyCanvas)); }

        public void UpdateTextBox(string str="")
        {
            if (string.IsNullOrEmpty(str)) return;
            //if (IsPointInImage(mouseMovePoint))
            //    PosinText.Text = $"X:{Convert.ToInt32(CurrentPoint.X)},Y:{Convert.ToInt32(CurrentPoint.Y)}";
            //else
            //    PosinText.Text = $"已经超出图片坐标X:{Convert.ToInt32(mouseMovePoint.X)}, Y:{Convert.ToInt32(mouseMovePoint.Y)}";
            PosinText.Text = str;
            PosinText.Background = new SolidColorBrush(Colors.Red);
            Canvas.SetLeft(PosinText, mouseMovePoint.X);
            Canvas.SetTop(PosinText, mouseMovePoint.Y + 5);
        }


        #endregion
    }
}
