using System.Collections.ObjectModel;
using Labs.Interfaces;
using Labs.Models;

namespace Labs.ViewModels.Tests
{
    public abstract class TestViewModel
    {
        public const string RunFirstTimer = "RunFirstTimer";

        protected FrameViewModel FrameViewModel;
        protected SettingsViewModel SettingsViewModel;
        protected ISettings Settings;
        public TimerViewModel Timer;
        public int Index;

        protected bool IsChickAble = true;

        public SettingsModel GetSettingsModel => SettingsViewModel.SettingsModel;
        public ObservableCollection<FrameModel> GetFrameModel => FrameViewModel.Models;
        public TimerModel GetTimerModel => Timer.TimerModel;

        protected void DisableTimer()
        {
            if (Timer != null) {
                Timer.TimerModel.TimerIsVisible = false;
                Timer.Index = null;
                Timer = null;
            }
        }
    }
}
