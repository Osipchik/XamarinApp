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
        private readonly EntryTypeTestViewModel _viewModel;
        private readonly TimerViewModel _timerViewModel;
       
        public EntryTypeTestPage(EntryTypeTestViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;
            Subscribe(viewModel.Index);
            BindingContext = _viewModel;
            Title = _viewModel.Index.ToString();
        }

        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e) =>
            ((ListView)sender).SelectedItem = null;

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
            //MessagingCenter.Subscribe<Page>(this, Constants.Check,
            //    (sender) => { _checkViewModel.CheckPageAsync(_testModel); });
        }
    }
}