using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using XamarinCommom.Models;
using ReactiveUI;
namespace ZFileApp.ViewModels
{
    public class NetDiskViewCellViewModel : ViewModelBase
    {
        private FolderModel _FolderInfo;
        public NetDiskViewCellViewModel(FolderModel folderModel)
        {
            _FolderInfo = folderModel;
        }


        public FolderModel FolderInfo
        {
            get { return _FolderInfo; }
            set { this.RaiseAndSetIfChanged(ref _FolderInfo, value); }
        }
       
    }
}
