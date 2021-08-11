using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Windows;

namespace ZFileWPFClient.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();

            this.MouseDown += (s, e) =>
            {
                if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
                {
                    this.DragMove();
                }
            };

            radioClose.Click += (s, e) =>
            {
                Environment.Exit(0);
            };

            WeakReferenceMessenger.Default.Register<string, string>(this, "NavigationPage", (sender, r) =>
            {
                if (!string.IsNullOrWhiteSpace(r))
                    this.DialogResult = false;
                else
                    this.DialogResult = true;
            });
        }
    }
}
