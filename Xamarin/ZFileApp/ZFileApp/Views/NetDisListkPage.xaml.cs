
using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Xamarin.Forms;
using ReactiveUI;
namespace ZFileApp.Views
{
    public partial class NetDisListkPage 
    {
        public NetDisListkPage()
        {
            InitializeComponent();
            this.WhenAnyValue(x => x.ViewModel.CellViewModel)
                           .BindTo(this, x => x.NuGetPackageListView.ItemsSource)
                           .DisposeWith(ViewBindings);
        }
    }
}
