using System;
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
        private int? lineToSwapFirst;
        public StackTypeTestPage(string path, string fileName, TimerViewModel testTimerViewModel)
        {
            InitializeComponent();

            _timerViewModel = testTimerViewModel;
            _stackViewModel = new StackTypeTestViewModel(path, fileName, testTimerViewModel);
            SetBindings();
        }
        private void SetBindings()
        {
            BindingContext = _stackViewModel.GetSettingsModel;
            ListView.BindingContext = _stackViewModel.FrameViewModel;
            GridProgress.BindingContext = _stackViewModel.TimerViewModel.TimerModel;
        }

        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }
        private void ListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            _stackViewModel.TapEvent(e.ItemIndex);
            SwapAsync(e.ItemIndex);
        }

        private async void SwapAsync(int index)
        {
            if (lineToSwapFirst == null) {
                lineToSwapFirst = index;
            }
            else if (index != lineToSwapFirst) {
                await Task.Run(() => _stackViewModel.Swap(lineToSwapFirst.Value, index));
                lineToSwapFirst = null;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (_timerViewModel == null) {
                MessagingCenter.Send<Page>(this, Constants.StopAllTimers);
                _stackViewModel.TimerViewModel.TimerRunAsync();
            }
        }
    }
}