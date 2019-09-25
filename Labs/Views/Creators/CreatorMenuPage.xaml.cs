using System;
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
        private readonly MenuCreatorViewModel _menuCreatorViewModel;

        public CreatorMenuPage(string path)
        {
            InitializeComponent();

            _menuCreatorViewModel = new MenuCreatorViewModel(path, this);
            BindingContext = _menuCreatorViewModel;
        }

        private async void SettingsButton_OnClickedAsync(object sender, EventArgs e)
        {
            await Device.InvokeOnMainThreadAsync(() =>
            {
                _tableVisible = !_tableVisible;
                if (_heightMax == 0){
                    _heightMax = 680;
                }

                FrameAnimation.RunShowOrHideAnimation(SettingsTableView, _heightMax, 0, _tableVisible, false);
            });
        }

        private void ListViewFiles_OnItemTapped(object sender, ItemTappedEventArgs e) =>
            _menuCreatorViewModel.OpenCreatingPage(e.ItemIndex);
        
        private void ListViewFiles_OnItemSelected(object sender, SelectedItemChangedEventArgs e) =>
            ((ListView)sender).SelectedItem = null;

        protected override bool OnBackButtonPressed() => _menuCreatorViewModel.OnBackButtonPressed();

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _menuCreatorViewModel.GetFilesAsync();
        }
    }
}