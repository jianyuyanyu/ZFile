using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace ZTAppFrameword.Template.Control
{
    /// <summary>
    ///********************************************
    /// 创建人      ：  Zt
    /// 创建时间    ：  2022/9/3 15:32:27 
    /// Description ：  自定义ComboBox控件
    ///********************************************/
    /// </summary>
    public class ZTComboBox:ComboBox
    {
        private TextBox EditableTextBox;
        private Popup DropDownPopup;
        /// <summary>
        /// 这是水印
        /// </summary>
        public string Watermark
        {
            get { return (string)GetValue(WatermarkProperty); }
            set { SetValue(WatermarkProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Watermark.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WatermarkProperty =
            DependencyProperty.Register("Watermark", typeof(string), typeof(ZTComboBox));

        /// <summary>
        /// 水印文字颜色
        /// </summary>
        public Brush WatermarkColor
        {
            get { return (Brush)GetValue(WatermarkColorProperty); }
            set { SetValue(WatermarkColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FocusedBorderBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WatermarkColorProperty =
            DependencyProperty.Register("WatermarkColor", typeof(Brush), typeof(ZTComboBox), new PropertyMetadata(Brushes.Transparent));


        /// <summary>
        /// 圆角
        /// </summary>
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(ZTComboBox));
        /// <summary>
        /// 分割线宽度(只有IsEditable开启才生效)
        /// </summary>
        public double Line
        {
            get { return (double)GetValue(LineProperty); }
            set { SetValue(LineProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Line.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LineProperty =
            DependencyProperty.Register("Line", typeof(double), typeof(ZTComboBox));

        /// <summary>
        /// 鼠标移入边框色
        /// </summary>
        public Brush HoverBorderBrush
        {
            get { return (Brush)GetValue(HoverBorderBrushProperty); }
            set { SetValue(HoverBorderBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HoverBorderBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HoverBorderBrushProperty =
            DependencyProperty.Register("HoverBorderBrush", typeof(Brush), typeof(ZTComboBox), new PropertyMetadata(Brushes.Transparent));


        /// <summary>
        /// 光标聚焦后的边框色
        /// </summary>
        public Brush FocusedBorderBrush
        {
            get { return (Brush)GetValue(FocusedBorderBrushProperty); }
            set { SetValue(FocusedBorderBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FocusedBorderBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FocusedBorderBrushProperty =
            DependencyProperty.Register("FocusedBorderBrush", typeof(Brush), typeof(ZTComboBox), new PropertyMetadata(Brushes.Transparent));


        /// <summary>
        /// 重写自定义指定项子控件
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is ZTComboBoxItem;
        }
        /// <summary>
        /// 抓取指定项控件并返回定制项控件
        /// </summary>
        /// <returns></returns>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ZTComboBoxItem();
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            EditableTextBox = GetTemplateChild("PART_EditableTextBox") as TextBox;
            DropDownPopup = GetTemplateChild("PART_Popup") as Popup;
            if (EditableTextBox != null)
            {
                EditableTextBox.TextChanged -= OnEditableTextBoxTextChanged;
                EditableTextBox.TextChanged += OnEditableTextBoxTextChanged;
            }
        }
        private void OnEditableTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            IsDropDownOpen = true;
        }

    }
}
