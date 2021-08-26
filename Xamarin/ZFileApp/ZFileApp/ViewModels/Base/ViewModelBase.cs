using System.Reactive.Disposables;
using Prism.Navigation;
using ReactiveUI;

namespace ZFileApp.ViewModels
{
    public class ViewModelBase : ReactiveObject, IDestructible, INavigationAware
    {
        protected CompositeDisposable Disposal = new CompositeDisposable();

        public void Destroy()
        {
            Disposal.Dispose();
        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters) { }
        public virtual void OnNavigatedTo(INavigationParameters parameters) { }
    }
}
