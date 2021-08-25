using DynamicData;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using XamarinCommom.Models;
using ZFileApp.Services;

namespace ZFileApp.ViewModels
{
    public class NetDiskPageViewModel : ViewModelBase
    {
        public readonly INavigationService _navigationService;

        private readonly IFolderService _folderservice;
        private readonly SourceCache<FolderModel, int> _folderCache =
        new SourceCache<FolderModel, int>(x => x.Id);

        public ReactiveCommand<FolderModel, Unit> PackageFileChild { get; set; }

        public NetDiskPageViewModel(INavigationService navigationService, IFolderService folderservice)
        {
            _navigationService = navigationService;
            _folderservice = folderservice;
            //集合处理
            var FolderChangeSet = _folderCache.Connect().RefCount();

            FolderChangeSet
                .Transform(x => new NetDiskViewCellViewModel(x))
                .Bind(out _folderModels)
                .DisposeMany()
                .Subscribe()
                .DisposeWith(Disposal);

            PackageFileChild = ReactiveCommand.CreateFromTask<FolderModel>(ExecuteFilePackage);

            GetFolderInfo();
        }

        public FolderModel CurrFolde = new FolderModel();


        public async void GetFolderInfo()=> await ExecuteFilePackage(new FolderModel() { Id = 2 });
    

        private async Task ExecuteFilePackage(FolderModel arg)
        {
            var ApiResqust = await _folderservice.GetSpaceInfo(2, arg.Id);
            if (ApiResqust.statusCode != 200) return;

            CurrFolde.Id = arg.Id;
            _folderCache.Clear();

            ApiResqust.data.FileInfo.ForEach(o =>
            _folderCache.AddOrUpdate(new FolderModel() { CRTime = o.CRDate.Value, format = o.FileExtendName, Size = o.FileSize, Id = o.ID, Name = o.Name+"."+ o.FileExtendName, Type = 1 }));
            ApiResqust.data.FolderInfo.ForEach(o => 
            _folderCache.AddOrUpdate(new FolderModel() { CRTime = o.CRDate.Value, format = "文件", Size = "", Id = o.ID, Name = o.Name, Type = 2, Remark = o.Remark }));
        }


        /// <summary>
        /// 文件集合
        /// </summary>
        private ReadOnlyObservableCollection<NetDiskViewCellViewModel> _folderModels;
        public ReadOnlyObservableCollection<NetDiskViewCellViewModel> FolderModels => _folderModels;



        private ObservableAsPropertyHelper<bool> _hasItems;
        private ObservableAsPropertyHelper<bool> _isRefreshing;
        public bool HasItems => _hasItems.Value;
        public bool IsRefreshing => _isRefreshing.Value;

        public string Instructions { get; set; }

    }
}
