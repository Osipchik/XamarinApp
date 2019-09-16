﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Labs.Helpers;
using Labs.Models;

namespace Labs.ViewModels.Tests
{
    class CheckTypeTestViewModel
    {
        public readonly FrameViewModel FrameViewModel;
        private readonly SettingsViewModel _settingsViewModel;

        public CheckTypeTestViewModel(string path, string fileName)
        {
            FrameViewModel = new FrameViewModel();
            _settingsViewModel = new SettingsViewModel();

            ReadFileAsync(path, fileName);
        }

        private async void ReadFileAsync(string path, string fileName)
        {
            var strings = DirectoryHelper.ReadStringsFromFile(path, fileName);
            await Task.Run(() => {
                _settingsViewModel.SetPageSettingsModel(strings[0], strings[1], strings[2]);
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
            await Task.Run(() => { FrameViewModel.SelectItem(index, false); });
        }
    }
}
