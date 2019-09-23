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

        public CheckTypeTestPage(string path, string fileName, TimerViewModel timer, TestModel model = null, int? index = null)
        {
            InitializeComponent();

            _timerViewModel = timer;
            _checkViewModel = new CheckTypeTestViewModel(path, fileName, timer, index);
            BindingContext = _checkViewModel;
            _testModel = model;
            Title = index.ToString();
            Subscribe(index);
        }

        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e) => 
            ((ListView)sender).SelectedItem = null;

        private void ListView_OnItemTapped(object sender, ItemTappedEventArgs e) => _checkViewModel.TapEvent(e.ItemIndex);
        
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (_timerViewModel == null && _checkViewModel.TimerViewModel != null) {
                MessagingCenter.Send<Page>(this, (string)Application.Current.Resources["StopAllTimers"]);
                _checkViewModel.TimerViewModel.TimerRunAsync();
            }
        }

        private void Subscribe(int? num)
        {
            if (num != null && num.Value == 1) {
                MessagingCenter.Subscribe<Page>(this, (string)Application.Current.Resources["RunFirstTimer"],
                    (sender) => { OnAppearing(); });
            }
            MessagingCenter.Subscribe<Page>(this, (string)Application.Current.Resources["Check"],
                (sender) => { _checkViewModel.CheckPageAsync(_testModel); });
        }
    }
}