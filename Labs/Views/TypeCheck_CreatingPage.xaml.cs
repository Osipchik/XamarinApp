using System;
using Labs.Helpers;
using Labs.Resources;
using Labs.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Labs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TypeCheckCreatingPage
    {
        private readonly string _fileName;
        private bool _isDeleteAvailable;
        private readonly CheckTypeCreatorViewModel _viewModel;
        private FrameAnimation _animation;

        public TypeCheckCreatingPage(string path, string fileName = "")
        {
            InitializeComponent();

            _fileName = fileName;
            _viewModel = new CheckTypeCreatorViewModel(path, _fileName, this, GridButtons);

            SetBindings();
            SetDeleteButton();
        }

        private void SetBindings()
        {
            BindingContext = _viewModel;
            ListView.BindingContext = _viewModel.FrameViewModel;
            SettingsLayout.BindingContext = _viewModel.GetSettingsModel;
        }
        private void SetDeleteButton()
        {
            _isDeleteAvailable = !string.IsNullOrEmpty(_fileName);
            if (_isDeleteAvailable) {
                ItemDeleteFile.Text = AppResources.Delete;
            }
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
            if (_animation == null) {
                _animation = new FrameAnimation(0, (uint)SettingsLayout.Height, 0);
            }
            _animation.RunShowOrHideAnimation(null, SettingsLayout, this, (int)SettingsLayout.Height == 0);
        }

        private void ChooseItemsToDelete_OnClicked(object sender, EventArgs e) => ChooseItems(-1);
        private void ChooseRightItems_OnClicked(object sender, EventArgs e) => ChooseItems(1);
        private void ChooseItems(int modificator)
        {
            HideOrShowAsync_OnClicked(this, EventArgs.Empty);
            GridButtons.IsVisible = true;
            SetActionToAdditionalButtons(ImageButtonCross, ImageButtonAccept, modificator);
        }

        private void SetActionToAdditionalButtons(ImageButton cross, ImageButton accept, int modificator)
        {
            _viewModel.Modificator = modificator;
            _viewModel.GetAdditionalButtons(cross, accept);
            _viewModel.FrameViewModel.DisableAllAsync();
        }
    }
}