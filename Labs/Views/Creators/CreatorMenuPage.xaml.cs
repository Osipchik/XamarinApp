using System;
using System.Threading.Tasks;
using Labs.Helpers;
using Labs.ViewModels.Creators;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Labs.Views.Creators
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreatorMenuPage
    {
        private bool _tableVisible = true;
        private uint _heightMax;
        private MenuCreatorViewModel _viewModel;
        private bool _refresh;

        public CreatorMenuPage(string testId = null)
        {
            InitializeComponent();
            InitializeAsync(testId);
        }

        private async void InitializeAsync(string testId )
        {
            await Task.Run(() =>
            {
                _viewModel = new MenuCreatorViewModel(this, testId);
                Device.BeginInvokeOnMainThread(()=>{ BindingContext = _viewModel; });
            });
        }

        private async void SettingsButton_OnClickedAsync(object sender, EventArgs e) => 
            await Device.InvokeOnMainThreadAsync(() => {
                _tableVisible = !_tableVisible;
                if (_heightMax == 0) {
                    _heightMax = 680;
                }

                FrameAnimation.RunShowOrHideAnimation(SettingsTableView, _heightMax, 0, _tableVisible, false);
            });
        
        private void ListViewFiles_OnItemTapped(object sender, ItemTappedEventArgs e) => 
            _viewModel.OpenCreatingPage(e.ItemIndex);

        private void ListViewFiles_OnItemSelected(object sender, SelectedItemChangedEventArgs e) =>
            ((ListView)sender).SelectedItem = null;

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (_refresh) {
                _viewModel.InitializeAsync(_viewModel.TestId);
                BindingContext = _viewModel;
            }
            else
            {
                _refresh = true;
            }
        }
    }
}