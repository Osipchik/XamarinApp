using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Labs.Helpers;
using Labs.Models;
using Labs.Resources;
using Xamarin.Forms;

namespace Labs.ViewModels.Creators
{
    public class StackTypeCreatorViewModel
    {
        private readonly string _path;
        private readonly string _fileName;
        public readonly FrameViewModel FrameViewModel;
        private readonly SettingsViewModel _settingsViewModel;
        private readonly Page _page;
        private readonly Grid _gridButtons;
        private int _modificator;
        public int Modificator
        {
            get => _modificator;
            set
            {
                _modificator = value;
                if (_modificator != 0)
                {
                    FrameViewModel.DisableLastItem();
                }
            }
        }
        public StackTypeCreatorViewModel(string path, string fileName, Page page, Grid gridButtons = null)
        {
            Modificator = 0;
            _page = page;
            _path = path;
            _fileName = fileName;
            _gridButtons = gridButtons;
            FrameViewModel = new FrameViewModel();
            _settingsViewModel = new SettingsViewModel();

            FileExist();
            SetCommands();
        }

        public ICommand AddItemCommand { protected set; get; }
        public ICommand DeleteCurrentFileCommand { protected set; get; }
        public ICommand SaveFileCommand { protected set; get; }
        public ICommand HideGridButtonsCommand { protected set; get; }
        public ICommand AcceptGridButtonCommand { protected set; get; }
        private void SetCommands()
        {
            AddItemCommand = new Command(() => { FrameViewModel.AddNewModelAsync(); });
            DeleteCurrentFileCommand = new Command(DeleteCurrentFile);
            SaveFileCommand = new Command(Save);
            HideGridButtonsCommand = new Command(HideGridButtons);
            AcceptGridButtonCommand = new Command(AcceptGridButton);
        }

        private void FileExist()
        {
            if (!string.IsNullOrEmpty(_fileName)) {
                ReadFile(_path, _fileName);
            }
            else {
                FrameViewModel.AddNewModelAsync();
            }
        }
        private async void ReadFile(string path, string fileName)
        {
            var strings = DirectoryHelper.ReadStringsFromFile(path, fileName);
            await Task.Run(() => _settingsViewModel.SetPageSettingsModel(strings[0], strings[1], strings[2]));
            FillFramesAsync(strings, 3);
        }
        private async void FillFramesAsync(IReadOnlyList<string> strings, int startIndex)
        {
            await Task.Run(() => {
                for (int i = startIndex; i < strings.Count; i++) {
                    FrameViewModel.Models.Add(new FrameModel {
                        ItemTextLeft = strings[i],
                        ItemTextRight = strings[++i],
                        BorderColor = FrameViewModel.GetColor(false)
                    });
                }
            });
        }

        public SettingsModel GetSettingsModel => _settingsViewModel.SettingsModel;
        public ObservableCollection<FrameModel> GetFrameModels => FrameViewModel.Models;


        public async void TapEvent(int index)
        {
            switch (Modificator)
            {
                case -1:
                    await Task.Run(() => { FrameViewModel.ItemToDelete(index); });
                    break;
                case 0: goto default;
                case 1:
                    await Task.Run(() => { FrameViewModel.RightItems(index); });
                    break;
                default:
                    // TODO: do anything with this
                    await Task.Run(() => { FrameViewModel.SelectItem(index); });
                    break;
            }
        }

        private async void Save()
        {
            if (await PageIsValid()) {
                DirectoryHelper.SaveFile((string)Application.Current.Resources["TestTypeStack"], _path, _fileName, await GetStringsToSave());
                await _page.Navigation.PopAsync(true);
            }
        }
        private async Task<List<string>> GetStringsToSave()
        {
            var stringsToSave = new List<string>();
            stringsToSave.AddRange(await _settingsViewModel.GetPageSettingsAsync());
            stringsToSave.AddRange(await Task.Run(GetFramesInfo));

            return stringsToSave;
        }
        private List<string> GetFramesInfo()
        {
            var textList = new List<string>();
            foreach (var model in FrameViewModel.Models)
            {
                textList.Add(model.ItemTextLeft);
                textList.Add(model.ItemTextRight);
            }

            return textList;
        }

        private async void DeleteCurrentFile()
        {
            if (!string.IsNullOrEmpty(_fileName)) {
                File.Delete(Path.Combine(_path, _fileName));
            }
            await _page.Navigation.PopAsync(true);
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
            var message = _settingsViewModel.CheckPageSettings();
            message += CheckFrames();
            return message;
        }
        private string CheckFrames()
        {
            var message = string.Empty;
            message += FrameViewModel.Models.Count < 1 ? AppResources.WarningAnswer : "";
            message += CheckFramesText();

            return message;
        }
        private string CheckFramesText()
        {
            var message = string.Empty;
            foreach (var model in FrameViewModel.Models)
            {
                if (!string.IsNullOrEmpty(model.ItemTextLeft)) {
                    model.ItemTextLeft = model.ItemTextLeft.Trim();
                }
                if (!string.IsNullOrEmpty(model.ItemTextLeft)) {
                    model.ItemTextRight = model.ItemTextRight.Trim();
                }

                message += string.IsNullOrEmpty(model.ItemTextLeft) ? AppResources.WarningAnswer : string.Empty;
                message += string.IsNullOrEmpty(model.ItemTextRight) ? AppResources.WarningAnswer : string.Empty;
            }

            return message;
        }

        private void HideGridButtons()
        {
            Modificator = 0;
            FrameViewModel.DisableAllAsync();
            _gridButtons.IsVisible = false;
        }
        private async void AcceptGridButton()
        {
            if (Modificator < 0) {
                await Task.Run(() => FrameViewModel.DeleteItems());
            }
            HideGridButtons();
        }
    }
}
