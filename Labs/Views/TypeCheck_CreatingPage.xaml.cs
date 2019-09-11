using System;
using System.IO;
using System.Threading.Tasks;
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
        private readonly CheckTypeViewModel _viewModel;

        public TypeCheckCreatingPage(string path, string fileName = "")
        {
            InitializeComponent();

            _fileName = fileName;
            _viewModel = new CheckTypeViewModel(path, _fileName);

            SetBindings();
            SetDeleteButton();
        }

        private void SetBindings()
        {
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

        private void Price_OnTextChanged(object sender, TextChangedEventArgs e) => PageHelper.CheckEntry(sender, e.NewTextValue);
        private void _entrySeconds_OnTextChanged(object sender, TextChangedEventArgs e) => PageHelper.CheckEntry(sender, e.NewTextValue);
        private void AddItem_OnClicked(object sender, EventArgs e) => _viewModel.FrameViewModel.AddNewModelAsync();

        private void HideOrShowAsync_OnClicked(object sender, EventArgs e)
        {
            _viewModel.RunHideOrShowAnimation(SettingsLayout, this, (int)SettingsLayout.Height == 0);
        }

        private async void SaveButton_OnClicked(object sender, EventArgs e)
        {
            if (await PageIsValid()) {
                _viewModel.Save();
                MessagingCenter.Send<Page>(this, Constants.CreatorListUpLoad);
                await Navigation.PopAsync(true);
            }
        }

        private async Task<bool> PageIsValid()
        {
            var message = await _viewModel.PageIsValidAsync();
            var returnValue = string.IsNullOrEmpty(message);
            if (!returnValue) {
                await DisplayAlert(AppResources.Warning, message, AppResources.Cancel);
            }

            return returnValue;
        }

        private async void ItemDeleteFileAsync_OnClicked(object sender, EventArgs e)
        {
            if (_isDeleteAvailable) {
                DirectoryHelper.DeleteFileAsync(this, Path.Combine(_viewModel.GetPath, _fileName));
                await Navigation.PopAsync(true);
            };
        }

        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }
        private void ListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            _viewModel.TapEvent(e.ItemIndex);
        }


        private void ChooseItemsToDelete_OnClicked(object sender, EventArgs e) => ChooseItems(-1);
        private void ChooseRightItems_OnClicked(object sender, EventArgs e) => ChooseItems(1);
        private void ChooseItems(int modificator)
        {
            HideOrShowAsync_OnClicked(this, EventArgs.Empty);
            GridAdditionalButtons.IsVisible = true;
            SetActionToAdditionalButtons(ImageButtonCross, ImageButtonAccept, modificator);
        }

        private void SetActionToAdditionalButtons(ImageButton cross, ImageButton accept, int modificator)
        {
            _viewModel.GetAdditionalButtons(cross, accept, modificator);
            _viewModel.FrameViewModel.DisableAllAsync();
            _viewModel.Modificator = modificator;
        }
        private void ImageButtonCross_OnPressed(object sender, EventArgs e)
        {
            HideAdditionalButton();
        }
        private void ImageButtonAccept_OnPressed(object sender, EventArgs eventArgs)
        {
            if (_viewModel.Modificator < 0) {
                _viewModel.FrameViewModel.DeleteItemsAsync();
            }
            HideAdditionalButton();
        }
        private void HideAdditionalButton()
        {
            _viewModel.Modificator = 0;
            _viewModel.FrameViewModel.DisableAllAsync();
            GridAdditionalButtons.IsVisible = false;
        }
    }
}