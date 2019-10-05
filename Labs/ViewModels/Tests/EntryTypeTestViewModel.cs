using System;
using System.Threading.Tasks;
using Labs.Data;
using Labs.Interfaces;
using Realms;

namespace Labs.ViewModels.Tests
{
    public class EntryTypeTestViewModel : TestViewModel
    {
        public EntryTypeTestViewModel(string questionId, TimerViewModel testTimeViewModel, string time, ISettings settings, int index)
        {
            Index = index;
            FrameViewModel = new FrameViewModel();
            SettingsViewModel = new SettingsViewModel();
            Timer = testTimeViewModel ?? new TimerViewModel(TimeSpan.Parse(time), Index);
            Initialize(questionId, testTimeViewModel);
        }

        private async void Initialize(string questionId, TimerViewModel testTimeViewModel)
        {
            await Task.Run(() => {
                using (var realm = Realm.GetInstance())
                {
                    var question = realm.Find<Question>(questionId);
                    SettingsViewModel.SetSettingsModel(question.QuestionText, question.Price, question.Time);
                    FrameViewModel.FillTestFrames(question.Contents, out _);
                }
            });
        }

        //public async void CheckPageAsync(TestModel testModel)
        //{
        //    EntryModel.IsReadOnly = true;
        //    await Task.Run(() => {
        //        if (EntryModel.Answer == EntryModel.RightAnswer) {
        //            EntryModel.BorderColor = GetColor(true);
        //            if (testModel == null) return;
        //            testModel.Price += int.Parse(GetSettingsModel.Price);
        //            testModel.RightAnswers++;
        //        }
        //        else EntryModel.BorderColor = GetColor(false);
        //    });

        //    await Task.Run(DisableTimer);
        //}
     
        //private Color GetColor(bool isRight) =>
        //    isRight ? (Color)Application.Current.Resources["ColorMaterialGreen"] 
        //            : (Color)Application.Current.Resources["ColorMaterialRed"];
    }
}
