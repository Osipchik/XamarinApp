﻿using System.Windows.Input;
using Labs.Helpers;
using Labs.Views;
using Labs.Views.Creators;
using Labs.Views.TestPages;
using Xamarin.Forms;

namespace Labs.ViewModels
{
    public class StartViewModel
    {
        private readonly string _path;
        private readonly Page _page;
        private bool _isClickAble;
        private readonly Button _changeButton;
        private readonly Button _startButton;
        public readonly SettingsViewModel SettingsViewModel;

        public StartViewModel(string path, Page page, Button changeButton, Button startButton)
        {
            _path = path;
            _page = page;
            _changeButton = changeButton;
            _startButton = startButton;
            SettingsViewModel = new SettingsViewModel();
            ReadSettings();
            SetCommands();
        }

        public ICommand ChangeButtonCommand { protected set; get; }
        public ICommand StartButtonCommand { protected set; get; }

        private void SetCommands()
        {
            ChangeButtonCommand = new Command(async () =>
            {
                ChangeButtonStyle_OnClick(_changeButton);
                if (_isClickAble) return;
                _isClickAble = true;
                await _page.Navigation.PushAsync(new CreatorMenuPage(_path));
            });

            StartButtonCommand = new Command(async () =>
            {
                ChangeButtonStyle_OnClick(_startButton);
                if (_isClickAble) return;
                _isClickAble = true;
                //await _page.Navigation.PushModalAsync(new TestPage111(_path));
                await _page.Navigation.PushModalAsync(new TestPage(_path));
            });
        }
        private void ReadSettings()
        {
            var settings = DirectoryHelper.ReadStringsFromFile(_path, Constants.SettingsFileTxt);
            if (settings != null) {
                SettingsViewModel.SetStartPageSettings(settings);
            }
        }

        public void StartPageCallBack()
        {
            _isClickAble = false;
            ChangeButtonStyle_OnCallBack(_changeButton);
            ChangeButtonStyle_OnCallBack(_startButton);
            MessagingCenter.Send<Page>(_page, Constants.HomeListUpload);
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