using System;
using Labs.Helpers;
using Labs.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Labs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreatorPage
    {
        private bool _tableVisible = true;
        private readonly MenuCreatorViewModel _menuCreatorViewModel;

        public CreatorPage(string path)
        {
            InitializeComponent();

            _menuCreatorViewModel = new MenuCreatorViewModel(path, this);
            SetBindings();
            Subscribe();
        }

        private void SetBindings()
        {
            BindingContext = _menuCreatorViewModel;
            GridButtons.BindingContext = _menuCreatorViewModel;
            ListViewFiles.BindingContext = _menuCreatorViewModel.InfoViewModel;
            SettingsTableView.BindingContext = _menuCreatorViewModel.GetSettingsModel;
        }

        private void SettingsButton_OnClickedAsync(object sender, EventArgs e)
        {
            _tableVisible = !_tableVisible;
            FrameAnimation.RunShowOrHideButtonAnimation(ButtonSettings, 350, _tableVisible);
            FrameAnimation.RunShowOrHideAnimation(SettingsTableView, 220, 0, _tableVisible);
        }

        private void Subscribe()
        {
            MessagingCenter.Subscribe<Page>(this, Constants.CreatorListUpLoad,
                (sender) => { _menuCreatorViewModel.GetFiles(); });
        }

        private void ListViewFiles_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            _menuCreatorViewModel.OpenCreatingPage(e.ItemIndex);
        }
        private void ListViewFiles_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        protected override bool OnBackButtonPressed()
        {
            return _menuCreatorViewModel.OnBackButtonPressed();
        }
    }
}