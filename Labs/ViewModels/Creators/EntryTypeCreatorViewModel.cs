using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Labs.Annotations;
using Labs.Helpers;
using Labs.Models;
using Labs.Resources;
using Xamarin.Forms;

namespace Labs.ViewModels.Creators
{
    public class EntryTypeCreatorViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private readonly string _path;
        private readonly string _fileName;
        private readonly Page _page;
        private readonly SettingsViewModel _settingsViewModel;

        private string _answer;
        public string Answer
        {
            get => _answer;
            set
            {
                _answer = value;
                OnPropertyChanged();
            }
        }

        public EntryTypeCreatorViewModel(string path, string fileName, Page page)
        {
            _path = path;
            _fileName = fileName;
            _page = page;
            _settingsViewModel = new SettingsViewModel();
            SetCommands();
            FileExist();
        }

        public ICommand DeleteCurrentFileCommand { protected set; get; }
        public ICommand SaveFileCommand { protected set; get; }
        private void SetCommands()
        {
            DeleteCurrentFileCommand = new Command(DeleteCurrentFile);
            SaveFileCommand = new Command(Save);
        }

        public SettingsModel GetSettingsModel => _settingsViewModel.SettingsModel;

        private void FileExist()
        {
            if (!string.IsNullOrEmpty(_fileName)) {
                Initialize(_path, _fileName);
            }
        }
        private async void Initialize(string path, string fileName)
        {
            var strings = DirectoryHelper.ReadStringsFromFile(path, fileName);
            await Task.Run(() => _settingsViewModel.SetPageSettingsModel(strings[0], strings[1], strings[2]));
            Answer = strings[3];
        }

        private async void Save()
        {
            if (await PageIsValid()) {
                DirectoryHelper.SaveFile(Constants.TestTypeEntry, _path, _fileName, await GetStringsToSave());
                await _page.Navigation.PopAsync(true);
            }
        }
        private async Task<List<string>> GetStringsToSave()
        {
            var stringsToSave = new List<string>();
            stringsToSave.AddRange(await _settingsViewModel.GetPageSettingsAsync());
            stringsToSave.Add(Answer);

            return stringsToSave;
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
            var message = CheckPage();
            var returnValue = string.IsNullOrEmpty(message);
            if (!returnValue) {
                await _page.DisplayAlert(AppResources.Warning, message, AppResources.Cancel);
            }

            return returnValue;
        }
        private string CheckPage()
        {
            var message = _settingsViewModel.CheckPageSettings();
            if (!string.IsNullOrEmpty(Answer)) {
                Answer = Answer.Trim();
            }

            message += string.IsNullOrEmpty(Answer) ? AppResources.WarningAnswer : string.Empty;
            return message;
        }
    }
}
