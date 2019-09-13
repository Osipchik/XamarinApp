using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Labs.Models;
using Xamarin.Forms;

namespace Labs.ViewModels
{
    public sealed class TimerViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Action ActionAfterFilling { get; }

        private const double Hours = 3_600_000;
        private const double Minutes = 60_000;
        private const double Seconds = 1_000;

        private readonly TimerModel _timerView;
        private double _updateRate, _step, _time;
        private bool _alive = true;

        public TimerViewModel(Action actionAfterFilling)
        {
            ActionAfterFilling = actionAfterFilling;
            _timerView = new TimerModel();
        }

        public double Progress
        {
            get { return _timerView.Progress; }
            set
            {
                _timerView.Progress = value;
                OnPropertyChanged();
            }
        }

        public string Time
        {
            get { return _timerView.Time; }
            set
            {
                _timerView.Time = value;
                OnPropertyChanged();
            }
        }

        public Rectangle Bounds
        {
            get { return _timerView.Bounds; }
            set
            {
                _timerView.Bounds = value;
                OnPropertyChanged();
            }
        }


        public async void SetTimer(string typeMeasuring, double time)
        {
            if (typeMeasuring == "0") time *= Hours;
            else if (typeMeasuring == "1") time *= Minutes;
            else if (typeMeasuring == "2") time *= Seconds;

            _time = time;
            _updateRate = 1000 / 60f; // 60Hz
            _step = 1 / (time / _updateRate);

            await Task.Run(()=>Device.StartTimer(TimeSpan.FromMilliseconds(_updateRate), TimerOnTick));
        }

        private bool TimerOnTick()
        {
            if (Progress < 1)
            {
                Device.BeginInvokeOnMainThread(() => Progress += _step);
                _time -= _updateRate;
                var timeSpan = TimeSpan.FromMilliseconds(_time);

                if (_time >= Hours)
                {
                    Bounds = new Rectangle(.5, .5, .16, 1);
                    Time = $"{timeSpan.Hours:00;00}:{timeSpan.Minutes:00;00}:{timeSpan.Seconds:00;00}";
                }
                else if (_time > Minutes)
                {
                    Bounds = new Rectangle(.5, .5, .1, 1);
                    Time = $"{timeSpan.Minutes:00;00}:{timeSpan.Seconds:00;00}";
                }
                else
                {
                    Bounds = new Rectangle(.5, .5, .05, 1);
                    Time = $"{timeSpan.Seconds:00;00}";
                }

                return _alive;
            }

            ActionAfterFilling.Invoke();
            return false;
        }

        public void TimerStop()
        {
            _alive = false;
        }

        public async void TimerRunAsync()
        {
            _alive = true;
            await Task.Run(()=>Device.StartTimer(TimeSpan.FromMilliseconds(_updateRate), TimerOnTick));
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
