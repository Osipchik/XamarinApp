using System;
using System.Threading.Tasks;
using Labs.Data;
using Labs.Interfaces;
using Realms;
using Xamarin.Forms;

namespace Labs.ViewModels.Tests
{
    public class CheckTypeTestViewModel : TestViewModel
    {
        public CheckTypeTestViewModel(string id, TimerViewModel testTimeViewModel, ISettings settings, int index)
        {
            Index = index;
            Settings = settings;
            SettingsViewModel = new SettingsViewModel();
            FrameViewModel = new FrameViewModel();

            InitializeAsync(testTimeViewModel, id);
        }

        private async void InitializeAsync(TimerViewModel testTimeViewModel, string id)
        {
            await Task.Run(() =>
            {
                using (var realm = Realm.GetInstance())
                {
                    var question = realm.Find<Question>(id);
                    Timer = testTimeViewModel ?? new TimerViewModel(TimeSpan.Parse(question.Time), Index);
                    SettingsViewModel.SetSettingsModel(question.QuestionText, question.Price, question.Time);
                    FrameViewModel.FillTestFramesAsync(question.Id);
                }
            });
        }

        public async void TapEvent(int index)
        {
            if (IsChickAble) {
                await Task.Run(() => { FrameViewModel.SelectItem(index, false); });
            }
        }

        public async void CheckPageAsync()
        {
            await Task.Run(() => {
                if (CheckModel()) {
                    var a = int.Parse(Settings.Price) + int.Parse(GetSettingsModel.Price);
                    Settings.Price = a.ToString();
                    GetSettingsModel.TotalCount += "1";
                }
            });
            IsChickAble = false;
            await Task.Run(DisableTimer);
        }

        private bool CheckModel()
        {
            var pageIsRight = true;
            foreach (var model in FrameViewModel.Models) {
                var isRight = model.BorderColor == (Color)Application.Current.Resources["ColorMaterialBlue"] == model.IsRight;
                model.BorderColor = FrameViewModel.GetColorOnCheck(isRight);
                pageIsRight = pageIsRight && isRight;
            }

            return pageIsRight;
        }
    }
}