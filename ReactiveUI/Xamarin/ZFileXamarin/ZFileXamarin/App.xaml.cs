using Splat;
using System;
using Xamarin.Forms;

using Xamarin.Forms.Xaml;
using ZFileXamarin.Base;

namespace ZFileXamarin
{
    public partial class App : Application
    {
      
        public App()
        {
            InitializeComponent();
            var bootstrapper = new AppBootstrapper();
            MainPage = bootstrapper.CreateMainPage();
        
        }

    

        protected override void OnStart()
        {
         
        }

        protected override void OnSleep()
        {

        }

        protected override void OnResume()
        {
        }
        
       
    }
}
