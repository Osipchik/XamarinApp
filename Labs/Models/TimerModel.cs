using System.ComponentModel;
using System.Runtime.CompilerServices;
using Labs.Annotations;

namespace Labs.Models
{
    public sealed class TimerModel : INotifyPropertyChanged
    {
        private string _time;
        public string Time
        {
            get => _time;
            set
            {
                _time = value;
                OnPropertyChanged();
            }
        }

        private double _progress;
        public double Progress
        {
            get => _progress;
            set
            {
                _progress = value;
                OnPropertyChanged();
            }
        }

        private bool _timerIsVisible;
        public bool TimerIsVisible
        {
            get => _timerIsVisible;
            set
            {
                _timerIsVisible = value;
                OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
