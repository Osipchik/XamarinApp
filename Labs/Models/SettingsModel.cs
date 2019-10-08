using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Labs.Annotations;
using Labs.Interfaces;
using Labs.ViewModels;

namespace Labs.Models
{
    public sealed class SettingsModel : INotifyPropertyChanged, ISettings
    {
        private TimeSpan _timeSpan;
        public TimeSpan TimeSpan
        {
            get => _timeSpan;
            set
            {
                _timeSpan = value;
                OnPropertyChanged();
            }
        }

        private string _seconds;
        public string Seconds
        {
            get => _seconds;
            set
            {
                _seconds = value;
                if (_seconds.Length > 2)
                {
                    _seconds = _seconds.Remove(1);
                }
                OnPropertyChanged();
            }
        }

        private string _totalCount;
        public string TotalCount
        {
            get => _totalCount ?? "00";
            set
            {
                _totalCount = SettingsViewModel.FixText(value, true);
                OnPropertyChanged();
            }
        }

        private string _totalPrice;
        public string TotalPrice
        {
            get => _totalPrice;
            set
            {
                _totalPrice = value;
                OnPropertyChanged();
            }
        }

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

        private string _price;
        public string Price
        {
            get => _price ?? "00";
            set
            {
                _price = SettingsViewModel.FixText(value);
                OnPropertyChanged();
            }
        }

        private string _question;
        public string Question
        {
            get => _question;
            set {
                _question = string.IsNullOrEmpty(value) ? "" : value;
                OnPropertyChanged();
            }
        }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        private string _subject;
        public string Subject
        {
            get => _subject;
            set
            {
                _subject = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
