using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Labs.Helpers;
using Labs.Models;
using Xamarin.Forms;

namespace Labs.ViewModels.Tests
{
    public class CheckTypeTestViewModel
    {
        private readonly FrameViewModel _frameViewModel;
        private readonly SettingsViewModel _settingsViewModel;
        public TimerViewModel TimerViewModel;
        private bool _isChickAble = true;

        public CheckTypeTestViewModel(string path, string fileName, TimerViewModel testTimeViewModel, int? index = null)
        {
            _frameViewModel = new FrameViewModel();
            _settingsViewModel = new SettingsViewModel();
            InitializeAsync(path, fileName, testTimeViewModel, index);
        }

        private async void InitializeAsync(string path, string fileName, TimerViewModel testTimeViewModel, int? index)
        {
            var strings = DirectoryHelper.ReadStringsFromFile(path, fileName);
            TimerViewModel = testTimeViewModel ?? new TimerViewModel(strings[0], index);
            await Task.Run(() => {
                _settingsViewModel.SetPageSettingsModel(strings[0], strings[1], strings[2]);
                FillFramesAsync(strings, strings[3], 4);
            });
        }

        private async void FillFramesAsync(IReadOnlyList<string> strings, string answers, int startIndex)
        {
            await Task.Run(() => {
                for (int i = startIndex; i < strings.Count; i++) {
                    _frameViewModel.Models.Add(new FrameModel {
                        ItemTextLeft = strings[i],
                        IsRight = answers[i - startIndex] == '0',
                        BorderColor = FrameViewModel.GetColor(false)
                    });
                }
            });
            await Task.Run(() => { _frameViewModel.Models.Shuffle(); });
        }

        public SettingsModel GetSettingsModel => _settingsViewModel.SettingsModel;
        public ObservableCollection<FrameModel> GetFrameModel => _frameViewModel.Models;
        public TimerModel GeTimerModel => TimerViewModel.TimerModel;

        public async void TapEvent(int index)
        {
            if (_isChickAble) {
                await Task.Run(() => { _frameViewModel.SelectItem(index, false); });
            }
        }

        public async void CheckPageAsync(TestModel testModel)
        {
            await Task.Run(() => {
                if (CheckModel() && testModel != null) {
                    testModel.Price += int.Parse(GetSettingsModel.Price);
                    testModel.RightAnswers++;
                }
            });
            _isChickAble = false;
            await Task.Run(DisableTimer);
        }

        private void DisableTimer()
        {
            if (TimerViewModel != null) {
                TimerViewModel.TimerModel.TimerIsVisible = false;
                TimerViewModel.Index = null;
                TimerViewModel = null;
            }
        }

        private bool CheckModel()
        {
            var pageIsRight = true;
            foreach (var model in _frameViewModel.Models) {
                var isRight = model.BorderColor == (Color)Application.Current.Resources["ColorMaterialBlue"] == model.IsRight;
                model.BorderColor = FrameViewModel.GetColorOnCheck(isRight);
                pageIsRight = pageIsRight && isRight;
            }

            return pageIsRight;
        }
    }
}