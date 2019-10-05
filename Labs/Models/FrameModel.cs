using System;
using System.ComponentModel;
using Xamarin.Forms;
using System.Runtime.CompilerServices;
using Labs.Annotations;
using Labs.Data;
using Labs.Interfaces;

namespace Labs.Models
{
    public sealed class FrameModel : INotifyPropertyChanged, IFrameElement
    {
        private Color _borderColor = Color.Accent;
        public Color BorderColor
        {
            get => _borderColor;
            set
            {
                _borderColor = value;
                OnPropertyChanged();
            }
        }

        public string MainText { get; set; } = string.Empty;

        private string _text;
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                OnPropertyChanged();
            }
        }

        public string TextUnderMain { get; set; } = string.Empty;

        public int QuestionType { get; set; }

        public bool IsRight { get; set; }

        public QuestionContent Content { get; set; }

        public string Id { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
