using Labs.Helpers;
using Labs.Models;
using Labs.ViewModels.Tests;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Labs.Views.TestPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntryTypeTestPage : ContentPage
    {
        private readonly EntryTypeTestViewModel _entryViewModel;
        private readonly TimerViewModel _timerViewModel;
        private readonly TestModel _testModel;
        public EntryTypeTestPage(string path, string fileName, TimerViewModel testTimerViewModel, TestModel model = null)
        {
            InitializeComponent();

            _timerViewModel = testTimerViewModel;
            _entryViewModel = new EntryTypeTestViewModel(path, fileName, testTimerViewModel);
            BindingContext = _entryViewModel;
            _testModel = model;
            Subscribe();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (_timerViewModel == null && _entryViewModel.TimerViewModel != null) {
                MessagingCenter.Send<Page>(this, Constants.StopAllTimers);
                _entryViewModel.TimerViewModel.TimerRunAsync();
            }
        }

        private void Subscribe()
        {
            MessagingCenter.Subscribe<Page>(this, "runFirstTimer",
                (sender) => { _entryViewModel.TimerViewModel.TimerRunAsync(); });
            MessagingCenter.Subscribe<Page>(this, Constants.Check,
                (sender) => { _entryViewModel.CheckPageAsync(_testModel); });
        }
    }
}