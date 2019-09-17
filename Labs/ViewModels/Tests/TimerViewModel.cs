using System;
using System.Collections.Generic;
using System.Text;
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
        private const double UpdateRate = 1000 / 60f;
        private double _step;
        private bool _timerIsAlive;

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
            var page = GetMilliseconds(timePage);
            if (page > 0) {
                _time = page;
                _step = 1 / (_time / UpdateRate);
                TimerModel.TimerIsVisible = true;
            }
            else {
                _time = 0;
                TimerModel.TimerIsVisible = false;
            }
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

        public void TimerStop() => _timerIsAlive = false;

        private bool TimerOnTick()
        {
            if (TimerModel.Progress < 1)
            {
                TimerModel.Progress += _step;
                _time -= UpdateRate;
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

                return _timerIsAlive;
            }

            return false;
        }

        private void Subscribe()
        {
            MessagingCenter.Subscribe<Page>(this, Constants.StopAllTimers,
                (sender) => { TimerStop(); });
        }
    }
}
