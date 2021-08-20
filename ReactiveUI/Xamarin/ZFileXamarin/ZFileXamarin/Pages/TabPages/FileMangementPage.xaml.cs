using ReactiveUI.XamForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZFileXamarin.ViewModel;

namespace ZFileXamarin.Pages.TabPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FileMangementPage : ReactiveContentView<FileMangementViewModel>
    {
        public FileMangementPage()
        {
            InitializeComponent();
        }
    }
}