using System;
using System.Threading.Tasks;
using Labs.Helpers;
using Labs.Models;
using Xamarin.Forms;

namespace Labs.ViewModels.Tests
{
    public class TimerViewModel
    {
        public readonly TimerModel TimerModel;
        private double _time;
        private double _step;
        private bool _timerIsAlive;

        private const double UpdateRate = 1000 / 60f;
        private const double Hours = 3_600_000;
        private const double Minutes = 60_000;
        private const double Seconds = 1_000;
        public TimerViewModel(string time)
        {
            TimerModel = new TimerModel();
            SetTime(time);
            Subscribe();
        }

        private void SetTime(string timePage)
        {
            var time = GetMilliseconds(timePage);
            if (time > 0) {
                InitializeTimer(time);
            }
            else {
                _time = 0;
                TimerModel.TimerIsVisible = false;
            }
        }

        private void InitializeTimer(double time)
        {
            _time = time;
            _step = 1 / (_time / UpdateRate);
            TimerModel.TimerIsVisible = true;
        }

        private double GetMilliseconds(string time)
        {
            var times = time.Split(':');
            var milliseconds = double.Parse(times[0]) * Hours;
            milliseconds += double.Parse(times[1]) * Minutes;
            milliseconds += double.Parse(times[2]) * Seconds;

            return milliseconds;
        }

        public async void TimerRunAsync()
        {
            if (_time > 0) {
                _timerIsAlive = true;
                await Device.InvokeOnMainThreadAsync(() =>
                    Device.StartTimer(TimeSpan.FromMilliseconds(UpdateRate), TimerOnTick));
            }
        }

        private bool TimerOnTick()
        {
            if (TimerModel.Progress < 1) {
                TimerModel.Progress += _step;
                _time -= UpdateRate;
                FormatTime();
                return _timerIsAlive;
            }

            return false;
        }

        private void FormatTime()
        {
            var timeSpan = TimeSpan.FromMilliseconds(_time);
            if (_time >= Hours) {
                TimerModel.Time = $"{timeSpan.Hours:00;00}:{timeSpan.Minutes:00;00}:{timeSpan.Seconds:00;00}";
            }
            else if (_time > Minutes) {
                TimerModel.Time = $"{timeSpan.Minutes:00;00}:{timeSpan.Seconds:00;00}";
            }
            else {
                TimerModel.Time = $"{timeSpan.Seconds:00;00}";
            }
        }

        public void TimerStop() => _timerIsAlive = false;

        private void Subscribe()
        {
            MessagingCenter.Subscribe<Page>(this, Constants.StopAllTimers,
                (sender) => { TimerStop(); });
        }

        public static async void DisableTimerAsync(TimerViewModel timerViewModel)
        {
            await Task.Run(() => {
                if (timerViewModel != null) {
                    timerViewModel.TimerStop();
                    timerViewModel.TimerModel.TimerIsVisible = false;
                }
            });
        }
    }
}
