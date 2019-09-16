using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Labs.Helpers;
using Labs.Models;

namespace Labs.ViewModels.Tests
{
    public class EntryTypeTestViewModel
    {
        private readonly SettingsViewModel _settingsViewModel;
        public string Answer { get; set; }

        public EntryTypeTestViewModel(string path, string fileName)
        {
            _settingsViewModel = new SettingsViewModel();

            ReadFileAsync(path, fileName);
        }

        private async void ReadFileAsync(string path, string fileName)
        {
            var strings = DirectoryHelper.ReadStringsFromFile(path, fileName);
            await Task.Run(() => {
                _settingsViewModel.SetPageSettingsModel(strings[0], strings[1], strings[2]);
                Answer = strings[3];
            });
        }

        public SettingsModel GetSettingsModel => _settingsViewModel.SettingsModel;
    }
}
