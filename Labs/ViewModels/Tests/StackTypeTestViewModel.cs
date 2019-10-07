using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Labs.Data;
using Labs.Helpers;
using Labs.Interfaces;
using Realms;

namespace Labs.ViewModels.Tests
{
    public class StackTypeTestViewModel : TestViewModel
    {
        private int? _lineToSwap;

        public StackTypeTestViewModel(string id, TimerViewModel testTimeViewModel, ISettings settings, int index)
        {
            Index = index;
            Settings = settings;

            Initialize(id, testTimeViewModel);
        }

        private async void Initialize(string questionId, TimerViewModel testTimeViewModel)
        {
            await Task.Run(() => {
                FrameViewModel = new FrameViewModel();
                SettingsViewModel = new SettingsViewModel();

                using (var realm = Realm.GetInstance())
                {
                    var question = realm.Find<Question>(questionId);
                    SettingsViewModel.SetSettingsModel(question.QuestionText, question.Price, question.Time);
                    Timer = testTimeViewModel ?? new TimerViewModel(TimeSpan.Parse(question.Time), Index);
                   
                    FillFramesAsync(question.Contents);
                }
            });
        }

        private async void FillFramesAsync(IEnumerable<QuestionContent> contents)
        {
            FrameViewModel.FillTestFrames(contents, out var textList);
            textList.ShuffleAsync();
            await Task.WhenAny(GetTask(textList));
        }

        private async Task GetTask(IList<string> textList)
        {
            for (int i = 0; i < textList.Count; i++) {
                FrameViewModel.Models[i].Text = textList[i];
            }

            FrameViewModel.Models.ShuffleAsync();
        }

        public async void TapEvent(int index)
        {
            if (IsChickAble) {
                await Task.Run(() => {
                    FrameViewModel.SelectItem(index, false);
                    TrySwap(index);
                });
            }
        }

        private void TrySwap(int index)
        {
            if (_lineToSwap == null) {
                _lineToSwap = index;
            }
            else if (index != _lineToSwap) {
                Swap(_lineToSwap.Value, index);
                _lineToSwap = null;
            }
        }

        private void Swap(int firstIndex, int secondIndex)
        {
            var a = FrameViewModel.Models[firstIndex].Text;
            FrameViewModel.Models[firstIndex].Text = FrameViewModel.Models[secondIndex].Text;
            FrameViewModel.Models[secondIndex].Text = a;
            FrameViewModel.DisableAllAsync();
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
                var isRight = model.MainText == model.Text;
                model.BorderColor = FrameViewModel.GetColorOnCheck(isRight);
                pageIsRight = pageIsRight && isRight;
            }

            return pageIsRight;
        }
    }

}
