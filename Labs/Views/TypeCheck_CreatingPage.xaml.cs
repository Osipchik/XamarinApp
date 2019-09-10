using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using Labs.Helpers;
using Labs.Models;
using Labs.Resources;
using Labs.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace Labs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TypeCheckCreatingPage
    {
        private readonly string _fileName;
        private readonly bool _isDeleteAvailable = false;
        private readonly CheckTypeViewModel _viewModel;

        public TypeCheckCreatingPage(string path)
        {
            InitializeComponent();

            _viewModel = new CheckTypeViewModel(path);
            BindingContext = _viewModel.FrameViewModel;
        }

        public TypeCheckCreatingPage(string path, string fileName) : this(path)
        {
            _fileName = fileName;
            _isDeleteAvailable = true;
            ItemDeleteFile.Text = AppResources.Delete;
        }

        private void Coast_OnTextChanged(object sender, TextChangedEventArgs e) => PageHelper.CheckEntry(sender, e.NewTextValue);
        private void _entrySeconds_OnTextChanged(object sender, TextChangedEventArgs e) => PageHelper.CheckEntry(sender, e.NewTextValue);
        private void AddItem_OnClicked(object sender, EventArgs e) => _viewModel.FrameViewModel.AddModel();

        private void HideOrShowAsync_OnClicked(object sender, EventArgs e) => 
            _viewModel.RunHideOrShowAnimation(Layout, this, (int)Layout.Height == 0);

        private async void SaveButton_OnClicked(object sender, EventArgs e)
        {
            if (await PageIsValid()) {
                _viewModel.Save();
            };
        }

        private async Task<bool> PageIsValid()
        {
            var message = await _viewModel.PageIsValidAsync(Question.Text, Price.Text, TimePicker.Time, Seconds.Text);
            var returnValue = string.IsNullOrEmpty(message);
            if (!returnValue) {
                await DisplayAlert(AppResources.Warning, message, AppResources.Cancel);
            }

            return returnValue;
        }

        private async void ItemDeleteFileAsync_OnClicked(object sender, EventArgs e)
        {
            if(!_isDeleteAvailable) return;
            await Task.Run(() => File.Delete(Path.Combine(_viewModel.GetPath, _fileName)));
            await Task.Run(() => MessagingCenter.Send<Page>(this, Constants.CreatorListUpLoad));
            await Navigation.PopAsync(true);
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
            GridAgree.IsVisible = true;
            SetActionToAdditionalButtons(ImageButtonCross, ImageButtonAccept, modificator);
        }

        private void SetActionToAdditionalButtons(ImageButton cross, ImageButton accept, int modificator)
        {
            _viewModel.GetAdditionalButtons(cross, accept, modificator);
            _viewModel.FrameViewModel.DisableAll();
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
            _viewModel.FrameViewModel.DisableAll();
            GridAgree.IsVisible = false;
        }


        private void Editor_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (_viewModel != null && !string.IsNullOrEmpty(e.NewTextValue)) {
                _viewModel.FrameViewModel.SetText(e.NewTextValue.Trim());
            }
        }
    }
}