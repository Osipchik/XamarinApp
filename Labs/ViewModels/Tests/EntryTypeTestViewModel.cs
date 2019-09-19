﻿using System.Drawing;
using System.Threading.Tasks;
using Labs.Helpers;
using Labs.Models;

namespace Labs.ViewModels.Tests
{
    public class EntryTypeTestViewModel
    {
        private readonly SettingsViewModel _settingsViewModel;
        public TimerViewModel TimerViewModel;
        public EntryTestModel EntryModel { get; }

        public EntryTypeTestViewModel(string path, string fileName, TimerViewModel testTimeViewModel)
        {
            _settingsViewModel = new SettingsViewModel();
            EntryModel = new EntryTestModel();
            Initialize(path, fileName, testTimeViewModel);
        }

        private async void Initialize(string path, string fileName, TimerViewModel testTimeViewModel)
        {
            var strings = DirectoryHelper.ReadStringsFromFile(path, fileName);
            TimerViewModel = testTimeViewModel ?? new TimerViewModel(strings[0]);
            await Task.Run(() => {
                _settingsViewModel.SetPageSettingsModel(strings[0], strings[1], strings[2]);
                EntryModel.RightAnswer = strings[3];
                EntryModel.BorderColor = Constants.Colors.ColorMaterialBlue;
            });
        }

        public SettingsModel GetSettingsModel => _settingsViewModel.SettingsModel;
        public TimerModel GeTimerModel => TimerViewModel.TimerModel;

        public async void CheckPageAsync()
        {
            EntryModel.IsReadOnly = true;
            EntryModel.BorderColor = GetColor(EntryModel.Answer == EntryModel.RightAnswer);
            await Task.Run(DeleteTimer);
        }

        private void DeleteTimer()
        {
            if (TimerViewModel != null) {
                TimerViewModel.DisableTimerAsync(TimerViewModel);
                TimerViewModel = null;
            }
        }

        private Color GetColor(bool isRight) =>
            isRight ? Constants.Colors.ColorMaterialGreen : Constants.Colors.ColorMaterialRed;
    }
}
