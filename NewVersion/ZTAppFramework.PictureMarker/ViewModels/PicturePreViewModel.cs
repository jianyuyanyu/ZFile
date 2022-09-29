using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using ZTAppFrameword.Template.Global;
using ZTAppFramework.PictureMarker.Extensions;
using ZTAppFramework.PictureMarker.PictureBasic;
using ZTAppFreamework.Stared.ViewModels;

namespace ZTAppFramework.PictureMarker.ViewModels
{
    public class PicturePreViewModel : ZTDialogViewModel
    {
        #region UI

        #endregion

        #region Command
        public DelegateCommand<Canvas> LoadedCommand { get; }
        public DelegateCommand<UserControl> SizeChangedCommand { get; }
        #endregion

        #region Service

        #endregion

        #region 属性
        PictureBase Picture;
        Canvas MyCanvas;
        string IMGPath;
        #endregion
        public PicturePreViewModel()
        {
            LoadedCommand = new DelegateCommand<Canvas>(Loaded);
        }



        #region Event

        private void Loaded(Canvas Param)
        {
            MyCanvas = Param;
            Picture = new PictureBase(Param);
            Picture.LoadImgFile(IMGPath);
        }

        #endregion

        #region Overrdie    
        public override void Cancel()
        {
           
        }

        public override void OnSave()
        {
            
        }

        public override void OnDialogOpened(IZTDialogParameter parameters)
        {
            base.OnDialogOpened(parameters);
            IMGPath = parameters.GetValue<string>("ImgPath");
        }
        #endregion
    }
}
