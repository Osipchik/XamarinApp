﻿using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Labs.Annotations;
using Labs.Data;
using Labs.Interfaces;
using Labs.Models;
using Labs.ViewModels.Creators;
using Labs.Views.Creators;
using Labs.Views.Popups;
using Labs.Views.TestPages;
using Realms;
using Rg.Plugins.Popup.Events;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace Labs.ViewModels
{
    public class StartViewModel : INotifyPropertyChanged
    {
        private bool _buttonsIsClickAble;
        private static bool isFirst = true;
        public INavigation Navigation { get; set; }
        public Button ChangeButton { get; set; }
        public Button StartButton { get; set; }
        public ISettings Settings { get; private set; }

        private readonly string _testId;

        public StartViewModel(string testId)
        {
            _buttonsIsClickAble = true;
            _testId = testId;
            InitializeAsync();
            SetCommands();
        }

        public ICommand ChangeButtonCommand { protected set; get; }
        public ICommand StartButtonCommand { protected set; get; }

        private async void InitializeAsync()
        {
            await Task.Run(() => {
                using (var realm = Realm.GetInstance())
                {
                    var model = realm.Find<TestModel>(_testId);
                    Settings = new SettingsModel
                    {
                        Time = model.Time,
                        Name = model.Name,
                        Subject = model.Subject,
                        TimeSpan = TimeSpan.Parse(model.Time),
                        TotalCount = model.Questions.Count.ToString(),
                        TotalPrice = model.Questions.Sum(item => int.Parse(item.Price)).ToString()
                    };
                    OnPropertyChanged(nameof(Settings));
                }
            });
        }

        private void SetCommands()
        {
            ChangeButtonCommand = new Command(async () =>
            {
                if (_buttonsIsClickAble && ChangeButton != null && Navigation != null) {
                    Loading();
                    ChangeButtonStyle_OnClickAsync(ChangeButton);
                    await Navigation.PushAsync(new CreatorMenuPage(_testId));
                }
            });

            StartButtonCommand = new Command(async () =>
            {
                if (_buttonsIsClickAble && StartButton != null && Navigation != null) {
                    ChangeButtonStyle_OnClickAsync(StartButton);
                    Loading();
                    await Navigation.PushModalAsync(new TestPage(_testId, Settings));
                }
            });
        }

        private async void Loading()
        {
            if (isFirst)
            {
                await PopupNavigation.Instance.PushAsync(new LoadingPopup());
                isFirst = false;
            }
        }

        public void StartPageCallBack()
        {
            _buttonsIsClickAble = true;
            InitializeAsync();
            ChangeButtonStyle_OnCallBack(ChangeButton);
            ChangeButtonStyle_OnCallBack(StartButton);
        }

        private void ChangeButtonStyle_OnClickAsync(Button button)
        {
            _buttonsIsClickAble = false;
            if (button != null)
            {
                button.BackgroundColor = (Color)Application.Current.Resources["ButtonTextColor"];
                button.TextColor = (Color)Application.Current.Resources["ButtonBackGroundColor"];
            }
        }

        private async void ChangeButtonStyle_OnCallBack(Button button)
        {
            await Device.InvokeOnMainThreadAsync(() => {
                _buttonsIsClickAble = true;
                button.BackgroundColor = (Color) Application.Current.Resources["ButtonBackGroundColor"];
                button.TextColor = (Color) Application.Current.Resources["ButtonTextColor"];
            });
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}