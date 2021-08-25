using System.Reactive.Disposables;
using ReactiveUI.XamForms;
using ZFileApp.ViewModels;
using ReactiveUI;
namespace ZFileApp.Views
{
    public partial class NetDiskViewCell : ContentViewBase<NetDiskViewCellViewModel>
    {
        public NetDiskViewCell()
        {
            InitializeComponent();
            this.OneWayBind(ViewModel, x => x.FolderInfo.Name, X => X.PackageName.Text);
            this.OneWayBind(ViewModel, x => x.FolderInfo.CRTime, x => x.CRTime.Text);
        }
    }
}
