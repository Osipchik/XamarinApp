using Labs.Helpers;
using Labs.Models;
using Labs.ViewModels.Tests;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Labs.Views.TestPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StackTypeTestPage : ContentPage
    {
        private readonly StackTypeTestViewModel _stackViewModel;
        private readonly TimerViewModel _timerViewModel;
        private readonly TestModel _testModel;
        public StackTypeTestPage(string path, string fileName, TimerViewModel timerViewModel, TestModel model = null, int? num = null)
        {
            InitializeComponent();

            _timerViewModel = timerViewModel;
            Title = num.ToString();
            _stackViewModel = new StackTypeTestViewModel(path, fileName, timerViewModel);
            BindingContext = _stackViewModel;
            _testModel = model;
            Subscribe(num);
        }

        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e) => 
            ((ListView)sender).SelectedItem = null;

        private void ListView_OnItemTapped(object sender, ItemTappedEventArgs e) => _stackViewModel.TapEvent(e.ItemIndex);

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (_timerViewModel == null && _stackViewModel.TimerViewModel != null) {
                MessagingCenter.Send<Page>(this, (string)Application.Current.Resources["StopAllTimers"]);
                _stackViewModel.TimerViewModel.TimerRunAsync();
            }
        }

        private void Subscribe(int? num)
        {
            if (num != null && num.Value == 1) {
                MessagingCenter.Subscribe<Page>(this, (string)Application.Current.Resources["RunFirstTimer"],
                    (sender) => { OnAppearing(); });
            }
            MessagingCenter.Subscribe<Page>(this, (string)Application.Current.Resources["Check"],
                (sender) => { _stackViewModel.CheckPageAsync(_testModel); });
        }
    }
}