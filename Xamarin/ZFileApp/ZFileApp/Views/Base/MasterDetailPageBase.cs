using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Text;
using ReactiveUI.XamForms;
using ZFileApp.ViewModels;

namespace ZFileApp.Views
{
  public abstract  class MasterDetailPageBase<TViewModel> :ReactiveMasterDetailPage<TViewModel>
        where TViewModel : ViewModelBase
    {

        protected readonly CompositeDisposable MasterDetailBindings = new CompositeDisposable();

        public void Destroy()
        {
            MasterDetailBindings?.Dispose();
        }
    }
}
