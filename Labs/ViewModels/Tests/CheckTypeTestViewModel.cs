using System;
using System.Threading.Tasks;
using Labs.Data;
using Labs.Helpers;
using Labs.Interfaces;
using Realms;
using Xamarin.Forms;

namespace Labs.ViewModels.Tests
{
    public class CheckTypeTestViewModel : TestViewModel
    {
        public CheckTypeTestViewModel(string questionId, TimerViewModel testTimeViewModel, string time, ISettings settings, int index)
        {
            Index = index;
            Settings = settings;
            FrameViewModel = new FrameViewModel();
            SettingsViewModel = new SettingsViewModel();
            Timer = testTimeViewModel ?? new TimerViewModel(TimeSpan.Parse(time), Index);
            Initialize(questionId);
        }

        private async void Initialize(string questionId)
        {
            await Task.Run(() => {
                using (var realm = Realm.GetInstance())
                {
                    var question = realm.Find<Question>(questionId);
                    SettingsViewModel.SetSettingsModel(question.QuestionText, question.Price, question.Time);
                    FrameViewModel.FillTestFrames(question.Contents, out _);
                    FrameViewModel.Models.Shuffle();
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
            await Task.Run(() =>
            {
                if (CheckModel())
                {
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