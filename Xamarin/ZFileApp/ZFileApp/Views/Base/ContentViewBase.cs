using System.Reactive.Disposables;

using ReactiveUI.XamForms;
using ZFileApp.ViewModels;

namespace ZFileApp.Views
{
    public abstract class ContentViewBase<TViewModel> : ReactiveContentView<TViewModel> 
           where TViewModel : ViewModelBase
    {
        protected readonly CompositeDisposable ViewCellBindings = new CompositeDisposable();
    }
}
