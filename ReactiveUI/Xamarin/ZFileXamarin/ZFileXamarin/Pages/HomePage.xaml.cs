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
    public partial class HomePage : ReactiveTabbedPage<HomeViewModel>
    {
        public HomePage()
        {
            InitializeComponent();
           
           
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
          
                
        }
    }
}