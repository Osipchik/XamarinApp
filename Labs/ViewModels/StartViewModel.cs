using System.Threading.Tasks;
using System.Windows.Input;
using Labs.Helpers;
using Labs.Models;
using Labs.Views.Creators;
using Labs.Views.TestPages;
using Xamarin.Forms;

namespace Labs.ViewModels
{
    public class StartViewModel
    {
        private readonly Page _page;
        private bool _isClickAble;
        private readonly Button _changeButton;
        private readonly Button _startButton;
        public readonly SettingsViewModel SettingsViewModel;

        public StartViewModel(string path, Page page, Button changeButton, Button startButton)
        {
            _page = page;
            _changeButton = changeButton;
            _startButton = startButton;
            SettingsViewModel = new SettingsViewModel();
            ReadSettingsAsync(path);
            SetCommands(path);
        }

        public ICommand ChangeButtonCommand { protected set; get; }
        public ICommand StartButtonCommand { protected set; get; }

        private void SetCommands(string path)
        {
            ChangeButtonCommand = new Command(async () =>
            {
                ChangeButtonStyle_OnClick(_changeButton);
                if (_isClickAble) return;
                _isClickAble = true;
                await _page.Navigation.PushAsync(new CreatorMenuPage(path));
            });
            StartButtonCommand = new Command(async () =>
            {
                ChangeButtonStyle_OnClick(_startButton);
                if (_isClickAble) return;
                _isClickAble = true;
                await _page.Navigation.PushModalAsync(new TestPage(path, SettingsViewModel.SettingsModel.Time));
            });
        }
        private async void ReadSettingsAsync(string path)
        {
            await Task.Run(() => {
                var settings = DirectoryHelper.ReadStringsFromFile(path, Constants.SettingsFileTxt);
                if (settings != null) {
                    SettingsViewModel.SetStartPageSettings(settings);
                }
            });
        }

        public SettingsModel GetSettingsModel => SettingsViewModel.SettingsModel;

        public void StartPageCallBack()
        {
            _isClickAble = false;
            ChangeButtonStyle_OnCallBack(_changeButton);
            ChangeButtonStyle_OnCallBack(_startButton);
        }

        private void ChangeButtonStyle_OnClick(Button button)
        {
            button.BackgroundColor = Constants.Colors.ColorMaterialBlue;
            button.TextColor = Color.White;
        }

        private void ChangeButtonStyle_OnCallBack(Button button)
        {
            button.BackgroundColor = Color.White;
            button.TextColor = Constants.Colors.ColorMaterialBlue;
        }
    }
}