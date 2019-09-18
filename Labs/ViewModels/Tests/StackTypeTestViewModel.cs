using System.Collections.Generic;
using System.Threading.Tasks;
using Labs.Helpers;
using Labs.Models;

namespace Labs.ViewModels.Tests
{
    class StackTypeTestViewModel
    {
        public readonly FrameViewModel FrameViewModel;
        private readonly SettingsViewModel _settingsViewModel;
        public TimerViewModel TimerViewModel;
        public StackTypeTestViewModel(string path, string fileName, TimerViewModel testTimeViewModel)
        {
            FrameViewModel = new FrameViewModel();
            _settingsViewModel = new SettingsViewModel();

            Initialize(path, fileName, testTimeViewModel);
        }
        public SettingsModel GetSettingsModel => _settingsViewModel.SettingsModel;
        private async void Initialize(string path, string fileName, TimerViewModel testTimeViewModel)
        {
            var strings = DirectoryHelper.ReadStringsFromFile(path, fileName);
            TimerViewModel = testTimeViewModel ?? new TimerViewModel(strings[0]);
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

        public async void TapEvent(int index) => await Task.Run(() => { FrameViewModel.SelectItem(index, false); });

        public void Swap(int firstIndex, int secondIndex)
        {
            var a = FrameViewModel.Models[firstIndex].ItemTextRight;
            FrameViewModel.Models[firstIndex].ItemTextRight = FrameViewModel.Models[secondIndex].ItemTextRight;
            FrameViewModel.Models[secondIndex].ItemTextRight = a;
            FrameViewModel.DisableAllAsync();
        }
    }
}
