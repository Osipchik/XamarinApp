using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Labs.Helpers;
using Labs.Models;

namespace Labs.ViewModels.Tests
{
    public class CheckTypeTestViewModel
    {
        public readonly FrameViewModel FrameViewModel;
        private readonly SettingsViewModel _settingsViewModel;
        public TimerViewModel TimerViewModel;

        public CheckTypeTestViewModel(string path, string fileName, TimerViewModel testTimeViewModel)
        {
            FrameViewModel = new FrameViewModel();
            _settingsViewModel = new SettingsViewModel();

            Initialize(path, fileName, testTimeViewModel);
        }

        private async void Initialize(string path, string fileName, TimerViewModel testTimeViewModel)
        {
            var strings = DirectoryHelper.ReadStringsFromFile(path, fileName);
            TimerViewModel = testTimeViewModel ?? new TimerViewModel(strings[0]);
            await Task.Run(() => {
                _settingsViewModel.SetPageSettingsModel(strings[0], strings[1], strings[2]);
                FillFramesAsync(strings, strings[3], 4);
            });
        }

        private async void FillFramesAsync(IReadOnlyList<string> strings, string answers, int startIndex)
        {
            await Task.Run(() => {
                for (int i = startIndex; i < strings.Count; i++) {
                    FrameViewModel.Models.Add(new FrameModel {
                        ItemTextLeft = strings[i],
                        IsRight = answers[i - startIndex] == '0',
                        BorderColor = FrameViewModel.GetColor(false)
                    });
                }
            });
            await Task.Run(() => { FrameViewModel.Models.Shuffle(); });
        }

        public SettingsModel GetSettingsModel => _settingsViewModel.SettingsModel;
        public ObservableCollection<FrameModel> GetFrameModel => FrameViewModel.Models;
        public TimerModel GeTimerModel => TimerViewModel.TimerModel;

        public async void TapEvent(int index) => await Task.Run(() => { FrameViewModel.SelectItem(index, false); });

        public async void CheckPageAsync(TestModel testModel)
        {
            await Task.Run(() => {
                if (!CheckModel()) return;
                if (testModel == null) return;
                testModel.Price += int.Parse(GetSettingsModel.Price);
                testModel.RightAnswers++;
            });
            
            await Task.Run(() => TimerViewModel.DisableTimerAsync());
        }

        private bool CheckModel()
        {
            var pageIsRight = true;
            foreach (var model in FrameViewModel.Models) {
                var isRight = model.BorderColor == Constants.Colors.ColorMaterialBlue == model.IsRight;
                model.BorderColor = FrameViewModel.GetColorOnCheck(isRight);
                pageIsRight = pageIsRight && isRight;
            }

            return pageIsRight;
        }
    }
}