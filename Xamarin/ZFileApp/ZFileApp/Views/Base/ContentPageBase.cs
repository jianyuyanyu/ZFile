using System.Reactive.Disposables;
using Prism.Navigation;

using ReactiveUI.XamForms;
using ZFileApp.ViewModels;

namespace ZFileApp.Views
{
    public abstract class ContentPageBase<T> : ReactiveContentPage<T>, IDestructible
         where T : ViewModelBase
    {
        protected readonly CompositeDisposable ViewBindings = new CompositeDisposable();

        public void Destroy()
        {
            ViewBindings?.Dispose();
        }
    }
}
