﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Labs.Helpers;
using Labs.Models;
using Labs.Resources;
using Xamarin.Forms;

namespace Labs.ViewModels
{
    public class EntryTypeCreatorViewModel
    {
        private readonly string _path;
        private readonly string _fileName;
        private readonly Page _page;
        private readonly PageSettingsViewModel _settingsViewModel;
        public string Answer { get; set; }

        public EntryTypeCreatorViewModel(string path, string fileName, Page page)
        {
            _path = path;
            _fileName = fileName;
            _page = page;
            _settingsViewModel = new PageSettingsViewModel();
            SetCommands();
            FileExist();
        }

        public ICommand DeleteCurrentFileCommand { protected set; get; }
        public ICommand SaveFileCommand { protected set; get; }
        private void SetCommands()
        {
            DeleteCurrentFileCommand = new Command(() => PageHelper.DeleteCurrentFile(_path, _fileName, _page));
            SaveFileCommand = new Command(Save);
        }

        public PageSettingsModel GetSettingsModel => _settingsViewModel.SettingsModel;

        private void FileExist()
        {
            if (!string.IsNullOrEmpty(_fileName)) {
                ReadFileAsync(_path, _fileName);
            }
        }
        private async void ReadFileAsync(string path, string fileName)
        {
            var strings = DirectoryHelper.ReadStringsFromFile(path, fileName);
            await Task.Run(() => {
                _settingsViewModel.SetPageSettingsModel(strings[0], strings[1], strings[2]);
                Answer = strings[3];
            });
        }

        private async void Save()
        {
            if (await PageIsValid())
            {
                PageHelper.SaveCurrentFile(Constants.TestTypeEntry, _path, _fileName, _page, await GetStringsToSave());
            }
        }
        private async Task<List<string>> GetStringsToSave()
        {
            var stringsToSave = new List<string>();
            stringsToSave.AddRange(await _settingsViewModel.GetPageSettingsAsync());
            stringsToSave.Add(Answer);

            return stringsToSave;
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
