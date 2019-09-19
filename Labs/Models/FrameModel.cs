﻿using System.ComponentModel;
using Xamarin.Forms;
using System.Runtime.CompilerServices;
using Labs.Annotations;

namespace Labs.Models
{
    public class FrameModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private Color _borderColor;
        public Color BorderColor
        {
            get => _borderColor;
            set
            {
                _borderColor = value;
                OnPropertyChanged();
            }
        }

        public string ItemTextLeft { get; set; }
        private string _itemTextRight;

        public string ItemTextRight
        {
            get => _itemTextRight;
            set
            {
                _itemTextRight = value;
                OnPropertyChanged();
            }
        }

        public bool IsRight { get; set; }

        public string RightString { get; set; }
    }
}
