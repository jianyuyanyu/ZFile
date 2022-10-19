﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ZTAppFramework.Template.Enums;

namespace ZTAppFramework.Template.Extensions
{
    /// <summary>
    ///  输入框帮助类型
    /// <para>创建者:YWK</para>
    /// <para>创建时间:2022-06-27 上午 10:55:39</para>
    /// </summary>
    public class ZTTextBoxHelper
    {
        /// <summary>
        /// 获取正则表达式
        /// <para>要想Regex生效InputType设置问Regex类</para>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetRegex(DependencyObject obj)
        {
            return (string)obj.GetValue(RegexProperty);
        }
        /// <summary>
        /// 设置正则表达式
        /// <para>要想Regex生效InputType设置问Regex类</para>
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetRegex(DependencyObject obj, string value)
        {
            obj.SetValue(RegexProperty, value);
        }

        // Using a DependencyProperty as the backing store for Regex.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RegexProperty =
            DependencyProperty.RegisterAttached("Regex", typeof(string), typeof(ZTTextBoxHelper));

        /// <summary>
        /// 获取输入类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Enums.InputType GetInputType(DependencyObject obj)
        {
            return (Enums.InputType)obj.GetValue(InputTypeProperty);
        }
        /// <summary>
        /// 设置输入类型
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetInputType(DependencyObject obj, Enums.InputType value)
        {
            obj.SetValue(InputTypeProperty, value);
        }

        // Using a DependencyProperty as the backing store for InputType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InputTypeProperty =
            DependencyProperty.RegisterAttached("InputType", typeof(Enums.InputType), typeof(ZTTextBoxHelper), new PropertyMetadata(OnIsInputTypeChanged));


        private static void OnIsInputTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox text)
            {
                text.PreviewTextInput -= Text_PreviewTextInput;
                text.PreviewTextInput += Text_PreviewTextInput;
                text.PreviewKeyDown -= Text_KeyDown;
                text.PreviewKeyDown += Text_KeyDown;
            }
        }

        private static void Text_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private static void Text_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            switch (GetInputType(sender as TextBox))
            {
                case Enums.InputType.Default:
                    e.Handled = false;
                    break;
                case Enums.InputType.Number:
                    e.Handled = new Regex(@"[^0-9|\-|\.]").IsMatch(e.Text);
                    break;
                case Enums.InputType.Phone:
                    e.Handled = IsPhone(e);
                    break;
                case Enums.InputType.Regex:
                    e.Handled = new Regex(GetRegex(sender as TextBox)).IsMatch(e.Text);
                    break;
                default:
                    e.Handled = false;
                    break;
            }
        }
        /// <summary>
        /// 检验手机号
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private static bool IsPhone(System.Windows.Input.TextCompositionEventArgs e)
        {
            if ((e.OriginalSource as TextBox).Text.ToCharArray().Length > 10) return true;
            return System.Text.RegularExpressions.Regex.IsMatch(e.Text, @"[^(1)\d{10}$]");
        }
    }
}
