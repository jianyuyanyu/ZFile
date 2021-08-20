using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZFileXamarin.ViewModel;

namespace ZFileXamarin
{
    public partial class MainPage : ContentPage
    {
        public LogViewModel VM { get; } = new LogViewModel();
        public MainPage()
        {
            InitializeComponent();
            this.BindingContext = VM;
        }

    }
}
