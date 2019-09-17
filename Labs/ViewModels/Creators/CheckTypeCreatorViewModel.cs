using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Labs.Helpers;
using Labs.Models;
using Labs.Resources;
using Xamarin.Forms;

namespace Labs.ViewModels.Creators
{
    public class CheckTypeCreatorViewModel 
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

        public CheckTypeCreatorViewModel(string path, string fileName, Page page, Grid gridButtons = null)
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
            if (!string.IsNullOrEmpty(_fileName)){
                InitializeAsync(_path, _fileName);
            }
            else {
                FrameViewModel.AddNewModelAsync();
            }
        }
        private async void InitializeAsync(string path, string fileName)
        {
            await Task.Run(() =>
            {
                var strings = DirectoryHelper.ReadStringsFromFile(path, fileName);
                Task.Run(() => _settingsViewModel.SetPageSettingsModel(strings[0], strings[1], strings[2]));
                FillFramesAsync(strings, strings[3], 4);
            });
        }
        private async void FillFramesAsync(IReadOnlyList<string> strings, string answers, int startIndex)
        {
            await Task.Run(() => {
                for (int i = startIndex; i < strings.Count; i++) {
                    FrameViewModel.AddModel(strings[i], answers[i - startIndex] == '0');
                }
            });
        }

        public SettingsModel GetSettingsModel => _settingsViewModel.SettingsModel;

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
                    await Task.Run(() => { FrameViewModel.SelectItem(index); });
                    break;
            }
        }

        private async void Save()
        {
            if (await PageIsValid()) {
                DirectoryHelper.SaveFile(Constants.TestTypeCheck, _path, _fileName, await GetStringsToSave());
                MessagingCenter.Send<Page>(_page, Constants.CreatorListUpLoad);
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
            var answers = string.Empty;
            var textList = new List<string>();
            foreach (var model in FrameViewModel.Models) {
                answers += model.IsRight ? "0" : "1";
                textList.Add(model.ItemTextLeft);
            }

            var list = new List<string> { answers };
            list.AddRange(textList);

            return list;
        }

        private void DeleteCurrentFile()
        {
            if (!string.IsNullOrEmpty(_fileName)) {
                DirectoryHelper.DeleteFileAsync(_page, Path.Combine(_path, _fileName));
            }
            _page.Navigation.PopAsync(true);
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
            message += FrameViewModel.Models.Count < 1 ? "Add frame" : "";
            if (FrameViewModel.Models.Any(model => string.IsNullOrEmpty(model.ItemTextLeft))) {
                message += AppResources.WarningAnswer;
            }
            message += CheckFramesText();

            return message;
        }
        private string CheckFramesText()
        {
            var message = string.Empty;
            foreach (var model in FrameViewModel.Models) {
                if (!string.IsNullOrEmpty(model.ItemTextLeft)) {
                    model.ItemTextLeft = model.ItemTextLeft.Trim();
                }

                message += string.IsNullOrEmpty(model.ItemTextLeft) ? AppResources.WarningAnswer : string.Empty;
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