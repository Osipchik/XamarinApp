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
        public EntryTypeTestPage(string path, string fileName, TimerViewModel timerViewModel, TestModel model = null, int? num = null)
        {
            InitializeComponent();

            _timerViewModel = timerViewModel;
            Title = num.ToString();
            _entryViewModel = new EntryTypeTestViewModel(path, fileName, timerViewModel);
            BindingContext = _entryViewModel;
            _testModel = model;
            Subscribe(num);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (_timerViewModel == null && _entryViewModel.TimerViewModel != null) {
                MessagingCenter.Send<Page>(this, Constants.StopAllTimers);
                _entryViewModel.TimerViewModel.TimerRunAsync();
            }
        }

        private void Subscribe(int? num)
        {
            if (num != null && num.Value == 1) {
                MessagingCenter.Subscribe<Page>(this, Constants.RunFirstTimer,
                    (sender) => { OnAppearing(); });
            }
            MessagingCenter.Subscribe<Page>(this, Constants.Check,
                (sender) => { _entryViewModel.CheckPageAsync(_testModel); });
        }
    }
}