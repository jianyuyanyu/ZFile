using DynamicData;
using DynamicData.Binding;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using XamarinCommom.Models;
using ZFileApp.Services;

namespace ZFileApp.ViewModels
{
    public class NetDisListkPageViewModel : NavigationViewModelBase
    {

        public readonly SourceCache<FolderModel, int> _FolderCache = new SourceCache<FolderModel, int>(x => x.Id);

        ReadOnlyObservableCollection<NetDiskViewCellViewModel> _CellViewModel;

        private readonly INavigationService _navigationService;
        public readonly IFolderService _folderService;
        public IPageDialogService _pageDialogService { get; }

        private string _selectedTag;
        private string _Title;
        private NetDiskViewCellViewModel _SelectItem;

     

        public ReadOnlyObservableCollection<NetDiskViewCellViewModel> CellViewModel => _CellViewModel;
        public NetDisListkPageViewModel(INavigationService navigationService, IFolderService folderService, IPageDialogService pageDialogService)
        {
            _navigationService = navigationService;
            _folderService = folderService;
            _pageDialogService = pageDialogService;
            var FolderCache = _FolderCache.Connect().RefCount();

            var sortChanged =
              this.WhenAnyValue(x => x.SelectedTag)
                  .Where(x => !string.IsNullOrEmpty(x))
                  .Select(selectedTag =>
                      SortExpressionComparer<NetDiskViewCellViewModel>
                          .Ascending(x => x.FolderInfo.Id)
                          .ThenByAscending(x => x.FolderInfo.Name.Contains(selectedTag)));
            FolderCache
               .Transform(x => new NetDiskViewCellViewModel(x))
               .Sort(sortChanged)
               .Bind(out _CellViewModel)
               .DisposeMany()
               .Subscribe()
               .DisposeWith(Disposal);

            this.WhenAnyValue(X => X.SelectItem)
                .Where(x => x != null)
                .Subscribe(_ => NavigatePage())
                .DisposeWith(Disposal);

            //this.WhenAnyValue(x => x.SelectItem)
            //  .Where(x => x == null)
            //  .Subscribe(_ => _FolderCache.Clear())
            //  .DisposeWith(Disposal);
            //Title = "我的网盘";
            //GetFileInfo();
        }


        async void NavigatePage()
        {
            if (_SelectItem.FolderInfo.Type == 1) {
                
                await _pageDialogService.DisplayAlertAsync("提示", "这是文件", "ok");
                SelectItem = null;
                return;
            } 
       
            int PageId = _SelectItem.FolderInfo.Id;
            string _Title = _SelectItem.FolderInfo.Name;
            
            SelectItem = null;
            await _navigationService.NavigateAsync($"NetDisListkPage?FolderId={PageId}&Title={_Title}");
        }


        public DelegateCommand NavigationPageBackCommand => new DelegateCommand(NavigationPageBack);

        private void NavigationPageBack()
        {
            _navigationService.GoBackAsync();
        }

        public string SelectedTag
        {
            get => _selectedTag;
            set => this.RaiseAndSetIfChanged(ref _selectedTag, value);
        }

        public NetDiskViewCellViewModel SelectItem
        {
            get => _SelectItem;
            set => this.RaiseAndSetIfChanged(ref _SelectItem, value);
        }

  

        public string Title
        {
            get => _Title;
            set => this.RaiseAndSetIfChanged(ref _Title, value);
        }



        public int CurrentFolderId { get; set; } = 2;
        async void GetFileInfo(int FolderId = 2, int Type = 2)
        {
            CurrentFolderId = FolderId;
            var ApiResult = await _folderService.GetSpaceInfo(Type, FolderId);
            if (ApiResult.statusCode != 200) return;
            ApiResult.data.FileInfo.ForEach(o => _FolderCache.AddOrUpdate(new FolderModel() { CRTime = o.CRDate.Value, format = o.FileExtendName, Size = o.FileSize, Id = o.ID, Name = o.Name+"."+ o.FileExtendName, Type = 1 }));
            ApiResult.data.FolderInfo.ForEach(o => _FolderCache.AddOrUpdate(new FolderModel() { CRTime = o.CRDate.Value, format = "文件", Size = "", Id = o.ID, Name = o.Name, Type = 2, Remark = o.Remark }));

        }
        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {

            if (parameters.ContainsKey("FolderId"))
            {
                if (parameters.ContainsKey("Title"))
                    Title = (string)parameters["Title"];
                
                string id = (string)parameters["FolderId"];
                CurrentFolderId = int.Parse(id);
                GetFileInfo(CurrentFolderId);
            }
            else
            {
                if (CurrentFolderId!=2)
                    return;
               
                Title = "我的网盘";
                GetFileInfo();
            }

        }

    }
}
