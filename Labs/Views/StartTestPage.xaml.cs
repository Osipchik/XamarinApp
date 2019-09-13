using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Labs.Helpers;
using Labs.Models;
using Labs.Resources;
using Labs.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Labs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartTestPage
    {
        private readonly string _path;
        private bool _isClickAble;
        private readonly PageSettingsViewModel _settingsViewModel;

        public StartTestPage(string path)
        {
            InitializeComponent();

            _settingsViewModel = new PageSettingsViewModel();
            _path = path;
            ReadSettings();
            BindingContext = _settingsViewModel.SettingsModel;
        }

        private void ReadSettings()
        {
            var settings = DirectoryHelper.ReadStringsFromFile(_path, Constants.SettingsFileTxt);
            if (settings != null) {
                _settingsViewModel.SetStartPageSettings(settings);
            }
        }

        private void Subscribe()
        {
            MessagingCenter.Subscribe<Page>(this, Constants.StartPageCallBack,
                (sender) => { StartPageCallBack(); });
        }

        private void StartPageCallBack()
        {
            //FillInfo();
            _isClickAble = false;
            ChangeButtonStyle_OnCallBack(ChangeButton);
            ChangeButtonStyle_OnCallBack(StartButton);
            MessagingCenter.Send<Page>(this, Constants.HomeListUpload);
        }

        private async void ChangeButton_OnClicked(object sender, EventArgs e)
        {
            ChangeButtonStyle_OnClick(ChangeButton);
            if (_isClickAble) return;
            _isClickAble = true;
            await Navigation.PushAsync(new CreatorPage(_path));
        }

        private async void StartButton_OnClicked(object sender, EventArgs e)
        {
            ChangeButtonStyle_OnClick(StartButton);
            if (_isClickAble) return;
            _isClickAble = true;
            await Navigation.PushModalAsync(new TestPage(_path));
        }

        private void ChangeButtonStyle_OnClick(Button button)
        {
            button.BackgroundColor = Constants.ColorMaterialBlue;
            button.TextColor = Color.White;
        }

        private void ChangeButtonStyle_OnCallBack(Button button)
        {
            button.BackgroundColor = Color.White;
            button.TextColor = Constants.ColorMaterialBlue;
        }
    }
}