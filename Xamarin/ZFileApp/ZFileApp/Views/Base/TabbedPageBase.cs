using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Text;
using ReactiveUI.XamForms;
using ZFileApp.ViewModels;

namespace ZFileApp.Views
{
  public abstract  class TabbedPageBase<TViewModel> : ReactiveTabbedPage<TViewModel>
        where TViewModel : ViewModelBase
    {

        protected readonly CompositeDisposable TabbedBindings = new CompositeDisposable();

        public void Destroy()
        {
            TabbedBindings?.Dispose();
        }
    }
}
