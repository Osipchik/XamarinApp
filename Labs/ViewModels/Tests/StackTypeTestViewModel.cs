using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Labs.Helpers;
using Labs.Models;

namespace Labs.ViewModels.Tests
{
    class StackTypeTestViewModel
    {
        public readonly FrameViewModel FrameViewModel;
        private readonly SettingsViewModel _settingsViewModel;
        public StackTypeTestViewModel(string path, string fileName)
        {
            FrameViewModel = new FrameViewModel();
            _settingsViewModel = new SettingsViewModel();

            ReadFileAsync(path, fileName);
        }
        public SettingsModel GetSettingsModel => _settingsViewModel.SettingsModel;
        private async void ReadFileAsync(string path, string fileName)
        {
            var strings = DirectoryHelper.ReadStringsFromFile(path, fileName);
            await Task.Run(() => {
                _settingsViewModel.SetPageSettingsModel(strings[0], strings[1], strings[2]);
                FillFramesAsync(strings, 3);
            });
        }
        private async void FillFramesAsync(IReadOnlyList<string> strings, int startIndex)
        {
            await Task.Run(() => {
                for (int i = startIndex; i < strings.Count; i++) {
                    FrameViewModel.AddModel(strings[i], false, strings[++i]);
                }
            });
        }

        public async void TapEvent(int index)
        {
            await Task.Run(() => { FrameViewModel.SelectItem(index, false); });
        }
    }
}
