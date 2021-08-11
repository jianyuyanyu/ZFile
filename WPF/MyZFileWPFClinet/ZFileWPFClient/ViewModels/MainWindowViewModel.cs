using Component;
using Prism.Mvvm;

namespace ZFileWPFClient.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "ZFile NetDisk";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private string _LeftTitel;
        public string LeftTitel
        {
            get { return _LeftTitel; }
            set { SetProperty(ref _LeftTitel, value); }
        }

        private string _ComPany;
        public string ComPany
        {
            get { return _ComPany; }
            set { SetProperty(ref _ComPany, value); }
        }

        public void Init()
        {
            LeftTitel = Contract.Title;
            ComPany = Contract.Company;
        }

        public MainWindowViewModel()
        {

        }
    }
}
