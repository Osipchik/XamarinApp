using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Labs.Helpers;
using Labs.Models;
using Labs.Resources;
using Labs.Views;
using Xamarin.Forms;

namespace Labs.ViewModels
{
    public class MenuCreatorViewModel
    {
        private readonly string _path;
        private readonly PageSettingsViewModel _settingsViewModel;
        private readonly Page _page;

        public readonly InfoViewModel InfoViewModel;

        public MenuCreatorViewModel(string path, Page page = null)
        {
            _path = path;
            _page = page;
            _settingsViewModel = new PageSettingsViewModel();
            
            InfoViewModel = new InfoViewModel(path);
            ReadSettings();
            GetFiles();
            CreateTempFolderAsync();
            SetCommands();
        }

        public ICommand CreateCheckTypePageCommand { protected set; get; }
        public ICommand CreateEntryTypePageCommand { protected set; get; }
        public ICommand CreateStackTypePageCommand { protected set; get; }
        public ICommand SaveTestCommand { protected set; get; }
        public ICommand DeleteTestCommand { protected set; get; }
        private void SetCommands()
        {
            CreateCheckTypePageCommand = new Command(async () =>
            {
                await _page.Navigation.PushAsync(new TypeCheckCreatingPage(_path));
            });

            CreateEntryTypePageCommand = new Command(async () =>
            {
                await _page.Navigation.PushAsync(new TypeEntryCreatingPage(_path));
            });

            CreateStackTypePageCommand = new Command(async () =>
            {
                await _page.Navigation.PushAsync(new TypeStackCreatingPage(_path));
            });

            SaveTestCommand = new Command(Save);
            DeleteTestCommand = new Command(DeleteFolderAsync);
        }

        private void ReadSettings()
        {
            var settings = DirectoryHelper.ReadStringsFromFile(_path, Constants.SettingsFileTxt);
            if (settings != null) {
                _settingsViewModel.SetMenuPageSettings(settings);
            }
        }
        public PageSettingsModel GetSettingsModel => _settingsViewModel.SettingsModel;

        public void GetFiles()
        {
            if (_page != null) {
                InfoViewModel.GetFilesModelAsync();
            }
        }

        public async void OpenCreatingPage(int index)
        {
            switch (DirectoryHelper.GetTypeName(InfoViewModel.InfoModels[index].Name))
            {
                case Constants.TestTypeCheck:
                    await _page.Navigation.PushAsync(new TypeCheckCreatingPage(_path, InfoViewModel.InfoModels[index].Name));
                    break;
                case Constants.TestTypeStack:
                    await _page.Navigation.PushAsync(new TypeStackCreatingPage(_path, InfoViewModel.InfoModels[index].Name));
                    break;

                case Constants.TestTypeEntry:
                    await _page.Navigation.PushAsync(new TypeEntryCreatingPage(_path, InfoViewModel.InfoModels[index].Name));
                    break;
            }
        }

        private async void DeleteFolderAsync()
        {
            await Task.Run(() => { Directory.Delete(_path, true); });
            await Task.Run(() => MessagingCenter.Send<Page>(_page, Constants.HomeListUpload));
            await _page.Navigation.PopToRootAsync(true);
        }

        private async void CreateTempFolderAsync()
        {
            if (!Directory.Exists(_path)) {
                await Task.Run(() => Directory.CreateDirectory(_path));
            }
        }

        private async void Save()
        {
            if (await PageIsValid()) {
                DirectoryHelper.SaveTestAsync(_path, await _settingsViewModel.GetPageSettingsAsync(true));
                MessagingCenter.Send<Page>(_page, Constants.HomeListUpload);
                if (_path.Contains(Constants.TempFolder)) GetFiles();
                else await _page.Navigation.PopToRootAsync(true);
            }
        }

        private async Task<bool> PageIsValid()
        {
            var message = await Task.Run(GetMessage);
            var returnValue = string.IsNullOrEmpty(message);
            if (!returnValue) {
                await _page.DisplayAlert(AppResources.Warning, message, AppResources.Cancel);
            }

            return returnValue;
        }
        private string GetMessage()
        {
            var message = _settingsViewModel.CheckCreatorMenuPageSettings();
            message += InfoViewModel.InfoModels.Count < 1 ? "asd" : string.Empty;
            return message;
        }

        private string GetTotalPrice()
        {
            var totalPrice = 0;
            foreach (var price in InfoViewModel.InfoModels) {
                totalPrice += int.Parse(price.Detail);
            }

            return totalPrice.ToString();
        }



        public bool OnBackButtonPressed()
        {
            if (_path != Constants.TempFolder)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    var result = await _page.DisplayAlert(AppResources.Warning, AppResources.Escape, AppResources.Yes, AppResources.No);
                    if (!result) return;
                    MessagingCenter.Send<Page>(_page, Constants.StartPageCallBack);
                    await _page.Navigation.PopAsync(true);
                });
            }

            return true;
        }
    }
}
