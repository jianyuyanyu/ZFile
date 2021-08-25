using Xamarin.Forms;
using ZFileApp.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;

namespace ZFileApp.Views
{
    public partial class NetDiskPage
    {
        public NetDiskPage()
        {
            InitializeComponent();

            //绑定注释
            this.OneWayBind(ViewModel, x => x.Instructions, x => x.Instructions.Text)
           .DisposeWith(ViewBindings);

            //绑定注释显示
            this.OneWayBind(ViewModel, x => x.HasItems, x => x.Instructions.IsVisible)
                .DisposeWith(ViewBindings);

            ///绑定是否视图刷新
            this.OneWayBind(ViewModel, x => x.IsRefreshing, x => x.FolderListView.IsRefreshing)
             .DisposeWith(ViewBindings);

            this.WhenAnyValue(x => x.ViewModel.FolderModels)
              .BindTo(this, x => x.FolderListView.ItemsSource)
              .DisposeWith(ViewBindings);
            //绑定视图集合
            //this.WhenAnyValue(x=>x.ViewModel.)

        }
    }
}
