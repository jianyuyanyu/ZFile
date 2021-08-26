
using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Xamarin.Forms;
using ReactiveUI;
using ZFileApp.ViewModels;

namespace ZFileApp.Views
{
    public partial class NetDisListkPage 
    {
        public NetDisListkPage()
        {
            InitializeComponent();
            this.WhenAnyValue(x => x.ViewModel.CellViewModel)
                           .BindTo(this, x => x.FolderListView.ItemsSource)
                           .DisposeWith(ViewBindings);
            FolderListView.ItemTapped +=(s,e) => ViewModel.SelectItem = e.Item as NetDiskViewCellViewModel;
        }
    }
}
