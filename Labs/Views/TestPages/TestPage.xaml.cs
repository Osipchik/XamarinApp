using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Labs.Helpers;
using Labs.Models;
using Labs.ViewModels;
using Labs.ViewModels.Tests;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Labs.Views.TestPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TestPage : TabbedPage
    {
        private readonly TestViewModel _testViewModel;
        private readonly TimerViewModel _timerViewModel;
        private readonly TestModel _testModel;
        private readonly List<Page> _pages;
        private bool _isAble = true;

        public TestPage(string path, string testTime)
        {
            InitializeComponent();
            _testViewModel = new TestViewModel(this);
            _timerViewModel = testTime != Constants.TimeZero ? new TimerViewModel(testTime, -1) : null;
            _testModel = new TestModel();
            _pages = new List<Page>();
            FillPages(path);
            Subscribe();
        }

        private void FillPages(string path)
        {
            var infos = new InfoViewModel(path);
            infos.GetFilesModel();
            AddPagesAsync(infos.InfoModels, path);
        }

        private async void AddPagesAsync(IEnumerable<InfoModel> infoModels, string path)
        {
            await Task.Run(() => { AddTestPages(infoModels, path); });
            await Task.Run(() => { _timerViewModel?.TimerRunAsync(); });
        }

        private async void AddTestPages(IEnumerable<InfoModel> infoModels, string path)
        {
            foreach (var model in infoModels) {
                await Device.InvokeOnMainThreadAsync(() => {
                    _pages.Add(AddTestPage(path, model));
                    Children.Add(_pages.Last());
                });
            }
            await Device.InvokeOnMainThreadAsync(() => Children.Add(new ResultPage(_testModel)));
            if (_timerViewModel == null) {
                MessagingCenter.Send<Page>(this, Constants.RunFirstTimer);
            }
        }

        private Page AddTestPage(string path, InfoModel model)
        {
            Page page = null;
            var testType = DirectoryHelper.GetTypeName(model.Name);
            if (testType == Constants.TestTypeCheck) {
                page = new CheckTypeTestPage(path, model.Name, _timerViewModel, _testModel, Children.Count + 1);
            }
            else if (testType == Constants.TestTypeEntry) {
                page = new EntryTypeTestPage(path, model.Name, _timerViewModel, _testModel, Children.Count + 1);
            }
            else if (testType == Constants.TestTypeStack) {
                page = new StackTypeTestPage(path, model.Name, _timerViewModel, _testModel, Children.Count + 1);
            }

            return page;
        }

        protected override void OnPagesChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnPagesChanged(e);
            if (Children.Count > 1 && _timerViewModel == null && _isAble) {
                MessagingCenter.Send<Page>(this, Constants.StopAllTimers);
            }
        }
        
        protected override bool OnBackButtonPressed() => _testViewModel.OnBackButtonPressed();

        private void Subscribe()
        {
            MessagingCenter.Subscribe<object>(this, Constants.TimerIsEnd, GoToNextPage);
            MessagingCenter.Subscribe<object>(this, Constants.ReturnPages, ReturnPagesAsync);
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
                    _isAble = false;
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
            await Device.InvokeOnMainThreadAsync(() => {
                for (int i = 0; i < _pages.Count; i++) {
                    if (Children[i] != _pages[i]) {
                        Children.Insert(i, _pages[i]);
                    }
                }
            });
        }
    }
}