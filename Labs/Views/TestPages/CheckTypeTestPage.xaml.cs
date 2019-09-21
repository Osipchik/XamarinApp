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

        public CheckTypeTestPage(string path, string fileName, TimerViewModel timerViewModel, TestModel model = null, int? num=null)
        {
            InitializeComponent();

            _timerViewModel = timerViewModel;
            _checkViewModel = new CheckTypeTestViewModel(path, fileName, timerViewModel);
            BindingContext = _checkViewModel;
            _testModel = model;
            Title = num.ToString();
            Subscribe(num);
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

        private void Subscribe(int? num)
        {
            if (num != null && num.Value == 1) {
                MessagingCenter.Subscribe<Page>(this, Constants.RunFirstTimer,
                    (sender) => { OnAppearing(); });
            }
            MessagingCenter.Subscribe<Page>(this, Constants.Check,
                (sender) => { _checkViewModel.CheckPageAsync(_testModel); });
        }
    }
}