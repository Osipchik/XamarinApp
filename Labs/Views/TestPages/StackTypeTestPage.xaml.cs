using System.Threading.Tasks;
using Labs.Helpers;
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
        private int? _lineToSwapFirst;
        private bool _isClickAble = true;
        public StackTypeTestPage(string path, string fileName, TimerViewModel testTimerViewModel)
        {
            InitializeComponent();

            _timerViewModel = testTimerViewModel;
            _stackViewModel = new StackTypeTestViewModel(path, fileName, testTimerViewModel);
            BindingContext = _stackViewModel;
            Subscribe();
        }

        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }
        private void ListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (_isClickAble) {
                _stackViewModel.TapEvent(e.ItemIndex);
                SwapAsync(e.ItemIndex);
            }
        }

        private async void SwapAsync(int index)
        {
            if (_lineToSwapFirst == null) {
                _lineToSwapFirst = index;
            }
            else if (index != _lineToSwapFirst) {
                await Task.Run(() => _stackViewModel.Swap(_lineToSwapFirst.Value, index));
                _lineToSwapFirst = null;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (_timerViewModel == null && _stackViewModel.TimerViewModel != null) {
                MessagingCenter.Send<Page>(this, Constants.StopAllTimers);
                _stackViewModel.TimerViewModel.TimerRunAsync();
            }
        }

        private void Subscribe()
        {
            MessagingCenter.Subscribe<Page>(this, "runFirstTimer",
                (sender) => { _stackViewModel.TimerViewModel.TimerRunAsync(); });
            MessagingCenter.Subscribe<Page>(this, Constants.Check,
                (sender) =>
                {
                    _stackViewModel.CheckPageAsync();
                    _isClickAble = false;
                });
        }
    }
}