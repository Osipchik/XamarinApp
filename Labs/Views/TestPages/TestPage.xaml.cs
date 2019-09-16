using Labs.Helpers;
using Labs.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Labs.Views.TestPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TestPage : TabbedPage
    {
        public TestPage(string path)
        {
            InitializeComponent();
            
            Fill(path);
        }

        private async void Fill(string path)
        {
            var infos = new InfoViewModel(path);
         
            infos.GetFilesModel();
            foreach (var model in infos.InfoModels)
            {
                switch (DirectoryHelper.GetTypeName(model.Name))
                {
                    case Constants.TestTypeCheck:
                        Children.Add(new CheckTypeTestPage(path, model.Name));
                        break;
                    case Constants.TestTypeEntry:
                        Children.Add(new EntryTypeTestPage(path, model.Name));
                        break;
                    case Constants.TestTypeStack:
                        Children.Add(new StackTypeTestPage(path, model.Name));
                        break;
                }
            }
        }
    }
}