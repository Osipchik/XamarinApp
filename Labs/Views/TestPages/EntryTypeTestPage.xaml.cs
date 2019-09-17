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
        }

        private void SetBindings()
        {
            BindingContext = _entryViewModel.GetSettingsModel;
            Editor.BindingContext = _entryViewModel.Answer;
            GridProgress.BindingContext = _entryViewModel.TimerViewModel.TimerModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (_timerViewModel == null) {
                MessagingCenter.Send<Page>(this, Constants.StopAllTimers);
                _entryViewModel.TimerViewModel.TimerRunAsync();
            }
        }
    }
}