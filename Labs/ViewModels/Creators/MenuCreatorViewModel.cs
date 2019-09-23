using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Labs.Helpers;
using Labs.Models;
using Labs.Resources;
using Labs.Views.Creators;
using Xamarin.Forms;

namespace Labs.ViewModels.Creators
{
    public class MenuCreatorViewModel
    {
        private readonly string _path;
        private readonly SettingsViewModel _settingsViewModel;
        private readonly Page _page;

        private readonly InfoViewModel _infoViewModel;

        public MenuCreatorViewModel(string path, Page page = null)
        {
            _path = path;
            _page = page;
            _settingsViewModel = new SettingsViewModel();

            _infoViewModel = new InfoViewModel(path);
            CreateTempFolderAsync();
            ReadSettingsAsync();
            GetFilesAsync();
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
                await _page.Navigation.PushAsync(new TypeCheckCreatingPage(_path)));

            CreateEntryTypePageCommand = new Command(async () =>
                await _page.Navigation.PushAsync(new TypeEntryCreatingPage(_path)));
            
            CreateStackTypePageCommand = new Command(async () =>
                await _page.Navigation.PushAsync(new TypeStackCreatingPage(_path)));
            
            SaveTestCommand = new Command(Save);
            DeleteTestCommand = new Command(DeleteFolderAsync);
        }

        private async void ReadSettingsAsync()
        {
            await Task.Run(() => {
                var settings = DirectoryHelper.ReadStringsFromFile(_path, Constants.SettingsFileTxt);
                if (settings != null) {
                    _settingsViewModel.SetMenuPageSettings(settings);
                }
            });
        }

        public SettingsModel GetSettingsModel => _settingsViewModel.SettingsModel;
        public ObservableCollection<InfoModel> GetInfoModels => _infoViewModel.InfoModels;

        public async void GetFilesAsync()
        {
            await Task.Run(() => {
                if (_page != null) {
                    _infoViewModel.GetFilesModel();
                }
            });
        }

        public async void OpenCreatingPage(int index)
        {
            var testType = DirectoryHelper.GetTypeName(_infoViewModel.InfoModels[index].Name);
            if (testType == Constants.TestTypeCheck) {
                await _page.Navigation.PushAsync(new TypeCheckCreatingPage(_path, _infoViewModel.InfoModels[index].Name));
            }
            else if(testType == Constants.TestTypeStack) {
                await _page.Navigation.PushAsync(new TypeStackCreatingPage(_path, _infoViewModel.InfoModels[index].Name));
            }
            else if (testType == Constants.TestTypeEntry) {
                await _page.Navigation.PushAsync(new TypeEntryCreatingPage(_path, _infoViewModel.InfoModels[index].Name));
            }
        }

        private async void DeleteFolderAsync()
        {
            if (Directory.Exists(_path)){
                Directory.Delete(_path, true);
            }
            CreateTempFolderAsync();
            await Device.InvokeOnMainThreadAsync(async ()=> 
                await _page.Navigation.PopToRootAsync(true));
        }

        private async void CreateTempFolderAsync()
        {
            await Task.Run(() => {
                if (!Directory.Exists(_path)) {
                    Directory.CreateDirectory(_path);
                }
            });
        }

        private async void Save()
        {
            await Task.Run(async () => {
                if (await PageIsValid()) {
                    DirectoryHelper.SaveTest(_path, await _settingsViewModel.GetPageSettingsAsync(true));
                    if (_path.Contains(Constants.TempFolder)) GetFilesAsync();
                    else {
                        await Device.InvokeOnMainThreadAsync(async ()=>
                            await _page.Navigation.PopToRootAsync(true));
                    }
                }
            });
        }

        private async Task<bool> PageIsValid()
        {
            var message = await Task.Run(GetMessage);
            var returnValue = string.IsNullOrEmpty(message);
            if (!returnValue) {
                await Device.InvokeOnMainThreadAsync(()=>
                    _page.DisplayAlert(AppResources.Warning, message, AppResources.Cancel));
            }

            return returnValue;
        }

        private string GetMessage()
        {
            var message = _settingsViewModel.CheckCreatorMenuPageSettings();
            message += _infoViewModel.InfoModels.Count < 1 ? AppResources.AddTestPage : string.Empty;
            return message;
        }

        private string GetTotalPrice()
        {
            var totalPrice = 0;
            foreach (var price in _infoViewModel.InfoModels) {
                totalPrice += int.Parse(price.Detail);
            }

            return totalPrice.ToString();
        }

        public bool OnBackButtonPressed()
        {
            if (_path != Constants.TempFolder) {
                Device.BeginInvokeOnMainThread(async () => {
                    var result = await _page.DisplayAlert(AppResources.Warning, AppResources.Escape, AppResources.Yes, AppResources.No);
                    if (!result) return;
                    await _page.Navigation.PopAsync(true);
                });
            }

            return true;
        }
    }
}
