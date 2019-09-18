using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Labs.Annotations;

namespace Labs.Models
{
    public class TestModel : INotifyPropertyChanged
    {
        private string _index;
        public string Title
        {
            get => $"page {_index}";
            set
            {
                _index = value;
                OnPropertyChanged();
            }
        }

        private bool _timerIsFinish;
        public bool TimerIsFinish
        {
            get => _timerIsFinish;
            set
            {
                _timerIsFinish = value;
                OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
