using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Labs.Data;
using Labs.Interfaces;
using Labs.Models;
using Realms;
using Xamarin.Forms;

namespace Labs.ViewModels.Tests
{
    public class TestPageViewModel
    {
        public const string RunFirstTimer = "RunFirstTimer";

        private readonly FrameViewModel _frameViewModel;
        private readonly SettingsViewModel _settingsViewModel;
        private readonly ISettings _settings;
        public TimerViewModel Timer;

        public TestPageViewModel(string id, TimerViewModel testTimeViewModel, string time, ISettings settings, int index)
        {
            _settings = settings;
            _settingsViewModel = new SettingsViewModel();
            _frameViewModel = new FrameViewModel();
            Timer = testTimeViewModel ?? new TimerViewModel(TimeSpan.Parse(time), index);
            InitializeAsync(testTimeViewModel, id);
        }

        public SettingsModel GetSettingsModel => _settingsViewModel.SettingsModel;
        public ObservableCollection<FrameModel> GetFrameModel => _frameViewModel.Models;
        public TimerModel GetTimerModel => Timer.TimerModel;
        public bool IsChickAble { get; private set; } = true;

        private async void InitializeAsync(TimerViewModel testTimeViewModel, string id)
        {
            await Task.Run(() => {
                using (var realm = Realm.GetInstance())
                {
                    var question = realm.Find<Question>(id);
                    _settingsViewModel.SetSettingsModel(question.QuestionText, question.Price, question.Time);
                    _frameViewModel.FillTestFrames(question.Contents, out _);
                }
            });
        }

        public void OnAppearing(Page page)
        {
            MessagingCenter.Send<Page>(page, TimerViewModel.StopAllTimers);
            Timer?.TimerRunAsync();
        }

        public async void TapEvent(int index)
        {
            if (IsChickAble) {
                await Task.Run(() => { _frameViewModel.SelectItem(index, false); });
            }
        }

        public async void SetResult(bool pageIsRight)
        {
            await Task.Run(() => {
                DisableTimer();
                if (pageIsRight)
                {
                    var a = int.Parse(_settings.Price) + int.Parse(GetSettingsModel.Price);
                    _settings.Price = a.ToString();
                    GetSettingsModel.TotalCount += "1";
                }
            });
            IsChickAble = false;
        }


        private void DisableTimer()
        {
            if (Timer != null)
            {
                Timer.TimerModel.TimerIsVisible = false;
                Timer.Index = null;
                Timer = null;
            }
        }
    }
}
