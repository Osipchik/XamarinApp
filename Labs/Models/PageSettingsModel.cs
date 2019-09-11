using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Labs.Annotations;

namespace Labs.Models
{
    public class PageSettingsModel : INotifyPropertyChanged
    {
        private TimeSpan _timeSpan;
        public TimeSpan TimeSpan
        {
            get { return _timeSpan; }
            set
            {
                _timeSpan = value;
                OnPropertyChanged();
            }
        }

        private string _seconds;
        public string Seconds
        {
            get => string.IsNullOrEmpty(_seconds) ? "00" : _seconds;
            set {
                _seconds = value;
                OnPropertyChanged();
            }
        }

        private string _price;
        public string Price
        {
            get => _price;
            set {
                _price = value;
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
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
