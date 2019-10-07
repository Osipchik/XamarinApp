using System.Threading.Tasks;
using Labs.Data;
using Labs.Interfaces;
using Labs.ViewModels.Tests;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Labs.Views.TestPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StackTypeTestPage : ContentPage
    {
        private StackTypeTestViewModel _viewModel;

        public StackTypeTestPage(string questionId, TimerViewModel testTimeViewModel, ISettings settings, int index)
        {
            InitializeComponent();

            InitializeAsync(questionId, testTimeViewModel, settings, index);
            Title = index.ToString();
        }

        private async void InitializeAsync(string id, TimerViewModel testTimeViewModel, ISettings settings, int index)
        {
            await Task.Run(() =>
            {
                _viewModel = new StackTypeTestViewModel(id, testTimeViewModel, settings, index);
                Subscribe(index);
                Device.BeginInvokeOnMainThread(() => { BindingContext = _viewModel; });
            });
        }

        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e) => 
            ((ListView)sender).SelectedItem = null;

        private void ListView_OnItemTapped(object sender, ItemTappedEventArgs e) => _viewModel.TapEvent(e.ItemIndex);

        protected sealed override void OnAppearing()
        {
            base.OnAppearing();
            if (_viewModel.Timer != null) {
                MessagingCenter.Send<Page>(this, TimerViewModel.StopAllTimers);
                _viewModel.Timer?.TimerRunAsync();
            }
        }

        private void Subscribe(int? num)
        {
            if (num.HasValue && num.Value == 1) {
                MessagingCenter.Subscribe<Page>(this, TestViewModel.RunFirstTimer,
                    (sender) => { OnAppearing(); });
            }
            MessagingCenter.Subscribe<Page>(this, ResultPage.Check,
                (sender) => { _viewModel.CheckPageAsync(); });
        }
    }
}