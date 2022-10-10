
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
using ZTAppFramework.PictureMarker.Extensions;
using ZTAppFramework.PictureMarker.Formulas;
using ZTAppFramework.PictureMarker.Model;

namespace ZTAppFramework.PictureMarker
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  WeiXiaolei
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



        #region 缩放拖拽控件
        public TransformGroup tfGroup = new TransformGroup();

        public ScaleTransform currentScale = new ScaleTransform();

        public TranslateTransform currentTranslate = new TranslateTransform();

        public TranslateTransform positionTranslate = new TranslateTransform();

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
        }
        void LoadImage(BitmapImage bitmap, int width = -1, int height = -1)
        {
            ImgInfo.CurrentBitmap = bitmap;
            if (ImageControl != null)
                MyCanvas.Children.Remove(ImageControl);


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
            FitWindow(false);
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
            //if (isEditing) { isEditing = false; }
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
        }

        void MyCanvas_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        void MyCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            mouseMovePoint = e.GetPosition(MyCanvas);
            UpdateTextBox();
            MouseMove();

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

        protected virtual void MouseMove()
        {

        }

        void MyCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MyCanvas.Cursor = Cursors.Cross;
            mouseUpPoint = e.GetPosition(MyCanvas);

            MouserUp();
            MyCanvas.Focus();
            ImageControl.Focusable = true;
            ImageControl.Focus();
            IsDrap = false;

        }

        protected virtual void MouserUp() {

            CircleData model = null;
            if (!CircleHelepr.IsMeetCirclMethod())
                CircleHelepr.AddPoint(relativePoint(mouseUpPoint));
            else
                model= CircleHelepr.Start_Compute_Three_Point_Draw_Cirle();
            if (model != null)
                MessageBox.Show($"获取圆的参数为X{model.CircleX}.Y:{model.CircleY}半径为:{model.CircleR}");
        }


        void MyCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mouseDownPoint = e.GetPosition(MyCanvas);
            MouseButtonState = e.ChangedButton;
            MouseDown();
            lastPoint = mouseDownPoint;
            MyCanvas.Cursor = Cursors.ScrollAll;
        }

        protected virtual void MouseDown() { IsDrap = true; }

        void MyCanvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
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
        public virtual void reRender()
        {
            currentScale.ScaleX = ScaleLevel;
            currentScale.ScaleY = ScaleLevel;

            if (ImageControl != null)
                ImageControl.RenderTransform = tfGroup;

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
        public void HideCoordinate()
        {
            MyCanvas.Cursor = Cursors.Arrow;
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
        void InitData()
        {
            ImgInfo = new PictureInfo();
            ThicknessAnima = new DoubleAnimation();
            ThicknessAnima.RepeatBehavior = new RepeatBehavior(0);
            currentScale.CenterX = currentScale.CenterY = 0;
            tfGroup.Children.Add(currentScale);
            tfGroup.Children.Add(currentTranslate);
            MyCanvas.Children.Add(PosinText);
        }
    }
}
