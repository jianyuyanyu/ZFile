using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WorkModel.WindosDlg.VM;

namespace WorkModel.WindosDlg
{
    /// <summary>
    /// PasteItem.xaml 的交互逻辑
    /// </summary>
    public partial class PasteItem : UserControl
    {
    
        public PasteItem(Prism.Ioc.IContainerProvider _provider, PasteitemsDto folder)
        {
            InitializeComponent();
            this.DataContext = new PasteItemViewModel(_provider, folder);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
