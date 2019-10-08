using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Labs.Data;
using Labs.Interfaces;
using Labs.Resources;
using Labs.ViewModels.Tests;
using Labs.Views.Popups;
using Realms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Labs.Views.TestPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TestPage : TabbedPage
    {
        public const string ReturnPages = "ReturnPages";
        public const string Check = "Check";
        private readonly ISettings _settings;
        private TimerViewModel _timer;
        private int _index;
        private readonly List<Page> _pages = new List<Page>();

        public TestPage(string testId, ISettings settings)
        {
            InitializeComponent();

            _settings = settings;
            InitializeAsync(testId);
        }

        private async void InitializeAsync(string id)
        {
            await Task.Run(() => {
                SetTimer(_settings.TimeSpan);
                using (var realm = Realm.GetInstance())
                {
                    foreach (var question in realm.Find<TestModel>(id).Questions) {
                        AddTestPage(question.Id, question.Type, question.Time);
                        _index++;
                    }
                }

                Device.BeginInvokeOnMainThread(() => { Children.Add(new ResultPage(_settings)); });
                MessagingCenter.Send<Page>(this, TestViewModel.RunFirstTimer);

                Subscribe();
            });
        }

        private void SetTimer(TimeSpan time)
        {
            _timer = null;
            if (!time.Equals(TimeSpan.Zero)){
                _timer = new TimerViewModel(time, -1);
            }
        }

        private void AddTestPage(string id, int type, string time)
        {
            switch (type)
            {
                case (int)Repository.Type.Check:
                    AddPage(new CheckTypeTestPage(id, _timer, time, _settings, _index));
                    break;
                case (int)Repository.Type.Stack:
                    AddPage(new StackTypeTestPage(id, _timer, _settings, _index));
                    break;
                case (int)Repository.Type.Entry:
                    AddPage(new EntryTypeTestPage(id, _timer, time, _settings, _index));
                    break;
            }
        }

        private void AddPage(Page page)
        {
            Device.BeginInvokeOnMainThread(() => { Children.Add(page); });
            _pages.Add(page);
        } 

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () => {
                var result = await DisplayAlert(AppResources.Warning, AppResources.Escape, AppResources.Yes, AppResources.No);
                if (!result) return;
                await Navigation.PopModalAsync(true);
            });

            return true;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Send<Page>(this, LoadingPopup.Finish);
        }

        private void Subscribe()
        {
            MessagingCenter.Subscribe<object>(this, TimerViewModel.TimerIsEnd, GoToNextPage);
            MessagingCenter.Subscribe<object>(this, ReturnPages, ReturnPagesAsync);
        }

        private void GoToNextPage(object sender)
        {
            var timer = (TimerViewModel)sender;
            if (timer.Index != null) {
                if (timer.Index < 0 || Children.Count == 2) {
                    while (Children.Count != 1) {
                        Children.RemoveAt(0);
                    }
                }
                else {
                    CurrentPage = GetNextPage((int)timer.Index + 1);
                    Children.Remove(_pages[(int)timer.Index]);
                }
            }

            if(Children.Count == 1) MessagingCenter.Unsubscribe<object>(this, TimerViewModel.TimerIsEnd);
        }

        private Page GetNextPage(int index)
        {
            Page page;
            if (Children.Count == 2) page = Children.Last();
            else if (index == _pages.Count) page = _pages.First();
            else page = _pages[index];

            return page;
        }

        private async void ReturnPagesAsync(object sender)
        {
            await Task.Run(() => {
                for (int i = 0; i < _pages.Count; i++) {
                    if (Children[i] != _pages[i]) {
                        var i1 = i;
                        Device.BeginInvokeOnMainThread(() => Children.Insert(i1, _pages[i1]));
                    }
                }
                MessagingCenter.Send<Page>(this, Check);
            });
        }
    }
}