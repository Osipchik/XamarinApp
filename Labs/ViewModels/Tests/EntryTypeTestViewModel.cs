using System;
using System.Threading.Tasks;
using Labs.Data;
using Labs.Interfaces;
using Realms;

namespace Labs.ViewModels.Tests
{
    public class EntryTypeTestViewModel : TestViewModel
    {
        public EntryTypeTestViewModel(string id, TimerViewModel testTimeViewModel, ISettings settings, int index)
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
            IsChickAble = true;
            await Task.Run(DisableTimer);
        }

        private bool CheckModel()
        {
            var isRight = FrameViewModel.Models[0].Text == FrameViewModel.Models[0].MainText;
            FrameViewModel.Models[0].BorderColor = FrameViewModel.GetColorOnCheck(isRight);

            return isRight;
        }
    }
}
