using ReactiveUI.XamForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZFileXamarin.ViewModel;

namespace ZFileXamarin.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ReactiveContentPage<HomeViewModel>
    {
        public HomePage()
        {
            InitializeComponent();
           
           
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ((NavigationPage)App.Current.MainPage).BarBackgroundColor =  Color.FromHex("F5F5F5");
            ((NavigationPage)App.Current.MainPage).BarTextColor = Color.Black;// Color.FromHex("1E90FF");
                
        }
    }
}