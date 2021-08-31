using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ZFileComponent.Verifys
{
    /// <summary>
    /// SliderVerify.xaml 的交互逻辑
    /// </summary>
    public partial class SliderVerify : UserControl
    {
        public SliderVerify()
        {
            InitializeComponent();

            this.Loaded += SliderverifLoad;
        }

        private void SliderverifLoad(object sender, RoutedEventArgs e)
        {
            
        }
        public string ImageUri
        {
            get { return (string)GetValue(ImageUriProperty); }
            set { SetValue(ImageUriProperty, value); }
        }

        public static readonly DependencyProperty ImageUriProperty =
            DependencyProperty.Register("ImageUri", typeof(string), typeof(SliderVerify), new PropertyMetadata(null, (p, d) =>
            {
                (p as SliderVerify).Restart();
            }));

        public bool Result
        {
            get { return (bool)GetValue(ResultProperty); }
            set { SetValue(ResultProperty, value); }
        }

        public static readonly DependencyProperty ResultProperty =
            DependencyProperty.Register("Result", typeof(bool), typeof(SliderVerify), new PropertyMetadata(false));

        private void Restart()
        {
            
        }

        private void Slider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {

        }
    }
}
