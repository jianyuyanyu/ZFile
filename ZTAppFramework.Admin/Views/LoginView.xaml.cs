using Prism.Services.Dialogs;
using System.Windows;

namespace ZTAppFramework.Admin.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window, IDialogWindow
    {
        public LoginView()
        {
            InitializeComponent();
        }

        public IDialogResult Result { get; set; }
    }
}
