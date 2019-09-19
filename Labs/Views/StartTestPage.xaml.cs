using Labs.Helpers;
using Labs.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Labs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartTestPage
    {
        public StartViewModel StartViewModel { get; }
        public StartTestPage(string path)
        {
            InitializeComponent();

            StartViewModel = new StartViewModel(path, this, ChangeButton, StartButton);
            ChangeButton.BindingContext = StartViewModel;
            StartButton.BindingContext = StartViewModel;
            BindingContext = this;
            Subscribe();
        }

        private void Subscribe()
        {
            MessagingCenter.Subscribe<Page>(this, Constants.StartPageCallBack,
                (sender) => { StartViewModel.StartPageCallBack(); });
        }
    }
}