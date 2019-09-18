using Labs.Helpers;
using Labs.ViewModels.Tests;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Labs.Views.TestPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CheckTypeTestPage : ContentPage
    {
        private readonly CheckTypeTestViewModel _checkViewModel;
        private readonly TimerViewModel _timerViewModel;
        public CheckTypeTestPage(string path, string fileName, TimerViewModel testTimerViewModel)
        {
            InitializeComponent();

            _timerViewModel = testTimerViewModel;
            _checkViewModel = new CheckTypeTestViewModel(path, fileName, testTimerViewModel);
            SetBindings();
            Subscribe();
        }

        private void SetBindings()
        {
            BindingContext = _checkViewModel.GetSettingsModel;
            ListView.BindingContext = _checkViewModel.FrameViewModel;
            GridProgress.BindingContext = _checkViewModel.TimerViewModel.TimerModel;
        }

        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e) => 
            ((ListView)sender).SelectedItem = null;
        
        private void ListView_OnItemTapped(object sender, ItemTappedEventArgs e) => _checkViewModel.TapEvent(e.ItemIndex);
        
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (_timerViewModel == null) {
                MessagingCenter.Send<Page>(this, Constants.StopAllTimers);
                _checkViewModel.TimerViewModel.TimerRunAsync();
            }
        }

        private void Subscribe()
        {
            MessagingCenter.Subscribe<Page>(this, "runFirstTimer", 
                (sender)=>{ _checkViewModel.TimerViewModel.TimerRunAsync(); });
        }
    }
}