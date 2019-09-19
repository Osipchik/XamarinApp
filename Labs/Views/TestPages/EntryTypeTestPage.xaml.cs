using Labs.Helpers;
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
        public EntryTypeTestPage(string path, string fileName, TimerViewModel testTimerViewModel)
        {
            InitializeComponent();

            _timerViewModel = testTimerViewModel;
            _entryViewModel = new EntryTypeTestViewModel(path, fileName, testTimerViewModel);
            SetBindings();
            Subscribe();
        }

        private void SetBindings()
        {
            BindingContext = _entryViewModel;
            //GridProgress.BindingContext = _entryViewModel.TimerViewModel.TimerModel;
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
                (sender) =>
                {
                    _entryViewModel.CheckPageAsync();
                });
        }
    }
}