using System.Collections.Generic;
using System.Collections.Specialized;
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
        public TestPage(string path, string testTime)
        {
            InitializeComponent();

            _testViewModel = new TestViewModel(this);
            _timerViewModel = testTime != Constants.TimeZero ? new TimerViewModel(testTime) : null;
            FillPages(path);
        }

        private async void FillPages(string path)
        {
            var infos = new InfoViewModel(path);
            infos.GetFilesModel();
            await Device.InvokeOnMainThreadAsync(() => AddPages(infos.InfoModels, path));
        }

        private void AddPages(IEnumerable<InfoModel> infoModels, string path)
        {
            foreach (var model in infoModels) {
                switch (DirectoryHelper.GetTypeName(model.Name))
                {
                    case Constants.TestTypeCheck:
                        Children.Add(new CheckTypeTestPage(path, model.Name, _timerViewModel));
                        break;
                    case Constants.TestTypeEntry:
                        Children.Add(new EntryTypeTestPage(path, model.Name, _timerViewModel));
                        break;
                    case Constants.TestTypeStack:
                        Children.Add(new StackTypeTestPage(path, model.Name, _timerViewModel));
                        break;
                }
            }

            _timerViewModel?.TimerRunAsync();
        }

        protected override void OnPagesChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnPagesChanged(e);
            if (Children.Count > 1) {
                MessagingCenter.Send<Page>(this, Constants.StopAllTimers);
            }
        }

        protected override bool OnBackButtonPressed() => _testViewModel.OnBackButtonPressed();
    }
}