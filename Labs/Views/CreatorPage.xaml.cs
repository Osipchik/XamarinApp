using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Labs.Helpers;
using Labs.Models;
using Labs.Resources;
using Labs.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Labs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreatorPage
    {
        private bool _tableVisible = true;
        private readonly InfoViewModel _infos;
        private readonly CreatorAnimationViewModel _animation;
        private readonly TestViewModel _testVM;

        public CreatorPage(string path)
        {
            InitializeComponent();
            _animation = new CreatorAnimationViewModel(350, (uint)SettingsTableView.HeightRequest, 0);
            _testVM = new TestViewModel(path);
            _infos = new InfoViewModel(path);
            if (path.Contains(Constants.TempFolder)) InitializeTemp();
            else InitializeExist();

            FillListViewAsync();
            Subscribe();
        }

        private void InitializeTemp()
        {
            _testVM.CreateTempFolderAsync();
            ItemClear.Clicked += Clear;
        }

        private async void InitializeExist()
        {
            await Task.Run(FillSettings);
            ItemClear.Clicked += DeleteAsync;
            ItemClear.Text = AppResources.Delete;
        }

        private void SettingsButton_OnClickedAsync(object sender, EventArgs e)
        {
            _tableVisible = !_tableVisible;
            _animation.RunShowOrHideAnimation(ButtonSettings, SettingsTableView, this, _tableVisible);
        }

        private async void FillListViewAsync() =>
            ListViewFiles.ItemsSource = await Task.Run(() => _infos.GetFilesInfo());
        
        private void FillSettings()
        {
            var settings = _testVM.ReadSettings();
            CellName.Text = settings.TestName;
            CellSubject.Text = settings.TestSubject;
            _timePicker.Time = settings.SettingSpan;
            _entrySeconds.Text = settings.Seconds;
        }

        private void Subscribe()
        {
            MessagingCenter.Subscribe<Page>(this, Constants.CreatorListUpLoad, (sender) => FillListViewAsync());
        }

        private async void ItemSave_OnClicked(object sender, EventArgs e)
        {
            if (!_infos.Any()) await DisplayAlert(AppResources.Warning, AppResources.CreatorQuestions, AppResources.Cancel);
            else if (await _testVM.SaveTestAsync(GetsSettings())) {
                Clear(this, EventArgs.Empty);
                MessagingCenter.Send<Page>(this, Constants.StartPageCallBack);
                if (!_testVM.GetPath.Contains(Constants.TempFolder)) await Navigation.PopAsync(true);
            }
            else await DisplayAlert(AppResources.Warning, AppResources.FillSettings, AppResources.Cancel);
        }

        private SettingsModel GetsSettings()
        {
            var settings = new SettingsModel {
                TestName = CellName.Text,
                TestSubject = CellSubject.Text,
                SettingSpan = _timePicker.Time,
                Seconds = _entrySeconds.Text
            };

            return settings;
        }

        private void Clear(object sender, EventArgs e)
        {
            _entrySeconds.Text = "00";
            _timePicker.Time = TimeSpan.Zero;
            CellName.Text = CellSubject.Text = string.Empty;
            ListViewFiles.ItemsSource = null;
        }

        private async void DeleteAsync(object sender, EventArgs e)
        {
            if (await DisplayAlert(AppResources.Warning, AppResources.DeleteAnswer, AppResources.Yes, AppResources.No)) {
                _testVM.DeleteFolderAsync(this);
                await Navigation.PopToRootAsync(true);
            }
        }

        private async void ListViewFiles_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            switch (CommonPageHelper.GetTypeName(_infos.InfosModel[e.ItemIndex].Name))
            {
                case Constants.TestTypeCheck:
                    await Navigation.PushAsync(new TypeCheckCreatingPage(_testVM.GetPath, _infos.InfosModel[e.ItemIndex].Name));
                    break;
                case Constants.TestTypeStack:
                    await Navigation.PushAsync(new TypeStackCreatingPage(_testVM.GetPath, _infos.InfosModel[e.ItemIndex].Name));
                    break;

                case Constants.TestTypeEntry:
                    await Navigation.PushAsync(new TypeEntryCreatingPage(_testVM.GetPath, _infos.InfosModel[e.ItemIndex].Name));
                    break;
            }
        }

        private void ListViewFiles_OnItemSelected(object sender, SelectedItemChangedEventArgs e) => 
            ((ListView)sender).SelectedItem = null;

        private async void TypeCheck_OnClicked(object sender, EventArgs e) =>
            await Navigation.PushAsync(new TypeCheckCreatingPage(_testVM.GetPath));

        private async void TypeEntry_OnClicked(object sender, EventArgs e) =>
            await Navigation.PushAsync(new TypeEntryCreatingPage(_testVM.GetPath));
        
        private async void TypeStack_OnClicked(object sender, EventArgs e) =>
            await Navigation.PushAsync(new TypeStackCreatingPage(_testVM.GetPath));

        protected override bool OnBackButtonPressed()
        {
            if (_testVM.GetPath != Constants.TempFolder) {
                Device.BeginInvokeOnMainThread(async () => {
                    var result = await DisplayAlert(AppResources.Warning, AppResources.Escape, AppResources.Yes, AppResources.No);
                    if (!result) return;
                    MessagingCenter.Send<Page>(this, Constants.StartPageCallBack);
                    Back();
                });
            }

            return true;
        }
        private async void Back() => await Navigation.PopAsync(true);

        private void _entrySeconds_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var text = e.NewTextValue;
            for (var i = 0; i < text.Length; i++) {
                if (text[i] == ',') text = text.Remove(i,1);
            }
            if (text.Length > 2) text = text.Remove(2);
            var entry = sender as Entry;
            entry.Text = text;
        }
    }
}