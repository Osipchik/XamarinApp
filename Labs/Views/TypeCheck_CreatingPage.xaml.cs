using System;
using Labs.Helpers;
using Labs.ViewModels.Creators;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Labs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TypeCheckCreatingPage
    {
        private readonly CheckTypeCreatorViewModel _viewModel;
        private bool _settingsIsVisible = true;
        private uint _heightMax;
        public TypeCheckCreatingPage(string path, string fileName = "")
        {
            InitializeComponent();

            _viewModel = new CheckTypeCreatorViewModel(path, fileName, this, GridButtons);
            SetBindings();
        }

        private void SetBindings()
        {
            BindingContext = _viewModel;
            ListView.BindingContext = _viewModel.FrameViewModel;
            SettingsLayout.BindingContext = _viewModel.GetSettingsModel;
        }

        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }
        private void ListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            _viewModel.TapEvent(e.ItemIndex);
        }

        private void HideOrShowAsync_OnClicked(object sender, EventArgs eventArgs)
        {
            _settingsIsVisible = !_settingsIsVisible;
            if (_heightMax == 0) _heightMax = (uint) SettingsLayout.Height;
            FrameAnimation.RunShowOrHideAnimation(SettingsLayout, _heightMax, 0, _settingsIsVisible);
        }

        private void ChooseItemsToDelete_OnClicked(object sender, EventArgs e) => ChooseItems(-1);
        private void ChooseRightItems_OnClicked(object sender, EventArgs e) => ChooseItems(1);
        private void ChooseItems(int modificator)
        {
            HideOrShowAsync_OnClicked(this, EventArgs.Empty);
            GridButtons.IsVisible = true;
            SetActionToGridButtons(ImageButtonAccept, modificator);
        }

        private void SetActionToGridButtons(ImageButton accept, int modificator)
        {
            _viewModel.Modificator = modificator;
            _viewModel.FrameViewModel.DisableAllAsync();
            if (modificator < 0) {
                accept.Opacity = 1;
                accept.Source = "CheckedRed.png";
            }
            else {
                accept.Opacity = 0.3;
                accept.Source = "CheckedBlack.png";
            }
        }
    }
}