using System.Collections.Generic;
using System.Collections.Specialized;
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
        public TestPage(string path, string testTime)
        {
            InitializeComponent();
            _testViewModel = new TestViewModel(this);
            _timerViewModel = testTime != Constants.TimeZero ? new TimerViewModel(testTime) : null;
            FillPages(path);

            _testModel = new TestModel();
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
            await Task.Run(()=> { _timerViewModel?.TimerRunAsync(); });
        }

        private async void AddTestPages(IEnumerable<InfoModel> infoModels, string path)
        {
            foreach (var model in infoModels) {
                await Device.InvokeOnMainThreadAsync(() => {
                    switch (DirectoryHelper.GetTypeName(model.Name))
                    {
                        case Constants.TestTypeCheck:
                            Children.Add(new CheckTypeTestPage(path, model.Name, _timerViewModel, _testModel,
                                Children.Count + 1));
                            break;
                        case Constants.TestTypeEntry:
                            Children.Add(new EntryTypeTestPage(path, model.Name, _timerViewModel, _testModel));
                            break;
                        case Constants.TestTypeStack:
                            Children.Add(new StackTypeTestPage(path, model.Name, _timerViewModel, _testModel));
                            break;
                    }
                });
            }
            await Device.InvokeOnMainThreadAsync(() => Children.Add(new ResultPage(_testModel)));
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