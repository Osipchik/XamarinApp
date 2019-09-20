using Labs.Helpers;
using Labs.Models;
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
        private readonly TestModel _testModel;

        public CheckTypeTestPage(string path, string fileName, TimerViewModel testTimerViewModel, TestModel model = null)
        {
            InitializeComponent();

            _timerViewModel = testTimerViewModel;
            _checkViewModel = new CheckTypeTestViewModel(path, fileName, testTimerViewModel);
            BindingContext = _checkViewModel;
            _testModel = model;
            Subscribe();
        }

        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e) => 
            ((ListView)sender).SelectedItem = null;

        private void ListView_OnItemTapped(object sender, ItemTappedEventArgs e) => _checkViewModel.TapEvent(e.ItemIndex);
        
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (_timerViewModel == null && _checkViewModel.TimerViewModel != null) {
                MessagingCenter.Send<Page>(this, Constants.StopAllTimers);
                _checkViewModel.TimerViewModel.TimerRunAsync();
            }
        }

        private void Subscribe()
        {
            MessagingCenter.Subscribe<Page>(this, "runFirstTimer", 
                (sender)=>{ _checkViewModel.TimerViewModel.TimerRunAsync(); });
            MessagingCenter.Subscribe<Page>(this, Constants.Check,
                (sender) => { _checkViewModel.CheckPageAsync(_testModel); });
        }
    }
}