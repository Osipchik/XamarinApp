using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Labs.Annotations;
using Xamarin.Forms;

namespace Labs.Models
{
    public sealed class TestInfoModel : INotifyPropertyChanged
    {
        public string TestId { get; set; }

        public string Name { get; set; }

        public string Subject { get; set; }

        public string Date { get; set; }

        private Color _labelNameColor = (Color)Application.Current.Resources["TextColor"];
        public Color LabelNameColor
        {
            get => _labelNameColor;
            set
            {
                _labelNameColor = value;
                OnPropertyChanged();
            }
        }

        private Color _labelSubjectColor = (Color)Application.Current.Resources["TextColor"];
        public Color LabelSubjectColor
        {
            get => _labelSubjectColor;
            set
            {
                _labelSubjectColor = value;
                OnPropertyChanged();
            }
        }

        private Color _labelDateColor = (Color)Application.Current.Resources["TextColor"];
        public Color LabelDateColor
        {
            get => _labelDateColor;
            set
            {
                _labelDateColor = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
