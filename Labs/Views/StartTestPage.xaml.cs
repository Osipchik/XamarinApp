using Labs.Helpers;
using Labs.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Labs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartTestPage
    {
        private readonly StartViewModel _startViewModel;

        public StartTestPage(string path)
        {
            InitializeComponent();

            _startViewModel = new StartViewModel(path, this, ChangeButton, StartButton);
            BindingContext = _startViewModel.SettingsViewModel.SettingsModel;
            ChangeButton.BindingContext = _startViewModel;
            StartButton.BindingContext = _startViewModel;
            Subscribe();
        }

        private void Subscribe()
        {
            MessagingCenter.Subscribe<Page>(this, Constants.StartPageCallBack,
                (sender) => { _startViewModel.StartPageCallBack(); });
        }
    }
}