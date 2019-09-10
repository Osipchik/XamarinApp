using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using System.Runtime.CompilerServices;
using System.Text;
using Labs.Annotations;

namespace Labs.Models
{
    public class FrameModel : INotifyPropertyChanged
    {
        private Color _borderColor;

        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                _borderColor = value;
                OnPropertyChanged();
            }
        }

        private string _itemTextLeft;
        public string ItemTextLeft
        {
            get { return _itemTextLeft; }
            set
            {
                _itemTextLeft = value;
                OnPropertyChanged();
            }
        }

        private bool _editorLeftIsReadOnly;
        public bool EditorLeftIsReadOnly
        {
            get { return _editorLeftIsReadOnly; }
            set
            {
                _editorLeftIsReadOnly = value;
                OnPropertyChanged();
            }
        }

        public bool isRight { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
