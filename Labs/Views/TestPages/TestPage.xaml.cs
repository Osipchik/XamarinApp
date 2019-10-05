using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Labs.Data;
using Labs.Interfaces;
using Labs.Resources;
using Labs.ViewModels.Tests;
using Realms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Labs.Views.TestPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TestPage : TabbedPage
    {
        public const string ReturnPages = "ReturnPages";
        private readonly ISettings _settings;
        private TimerViewModel _timer;

        private readonly List<Page> _pages = new List<Page>();

        public TestPage(string testId, ISettings settings)
        {
            InitializeComponent();
            _settings = settings;
            InitializeAsync(settings, testId);
        }

        private async void InitializeAsync(ISettings settings, string id)
        {
            await Task.Run(() => {
                SetTimer(settings.TimeSpan);
                using (var realm = Realm.GetInstance())
                {
                    foreach (var question in realm.Find<TestModel>(id).Questions) {
                        AddTestPage(question.Type, question.Id, question.Time);
                    }
                }

                Subscribe();
            });
        }

        private void SetTimer(TimeSpan time)
        {
            _timer = null;
            if (!time.Equals(TimeSpan.Zero)){
                _timer = new TimerViewModel(time);
            }
        }

        private void AddTestPage(int type, string questionId, string time)
        {
            var index = _pages.Count;
            switch (type)
            {
                case (int)Repository.Type.Check:
                    var checkViewModel = new CheckTypeTestViewModel(questionId, _timer, time, _settings, index);
                    AddPage(new CheckTypeTestPage(checkViewModel));
                    break;
                case (int)Repository.Type.Stack:
                    var stackViewModel = new StackTypeTestViewModel(questionId, _timer, time, _settings, index);
                    AddPage(new StackTypeTestPage(stackViewModel));
                    break;
                case (int)Repository.Type.Entry:
                    var entryViewModel = new EntryTypeTestViewModel(questionId, _timer, time, _settings, index);
                    AddPage(new EntryTypeTestPage(entryViewModel));
                    break;
            }
        }

        private void AddPage(Page page)
        {
            Device.BeginInvokeOnMainThread(() => Children.Add(page));
            _pages.Add(page);
            if (_pages.Count == int.Parse(_settings.TotalCount)) {
                Device.BeginInvokeOnMainThread(() => Children.Add(new ResultPage(_settings)));
                MessagingCenter.Send<Page>(this, TestViewModel.RunFirstTimer);
            }
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

        private void Subscribe()
        {
            MessagingCenter.Subscribe<object>(this, TimerViewModel.TimerIsEnd, GoToNextPage);
            MessagingCenter.Subscribe<object>(this, ReturnPages, ReturnPagesAsync);
        }

        private void GoToNextPage(object sender)
        {
            var timer = (TimerViewModel)sender;
            if (timer.Index != null) {
                if (timer.Index < 0) {
                    while (Children.Count != 1) {
                        Children.RemoveAt(0);
                    }
                }
                else {
                    CurrentPage = GetNextPage((int)timer.Index);
                    Children.Remove(_pages[(int)timer.Index - 1]);
                }
            }
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
            });
        }
    }
}