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
        private Color _itemColor;

        public Color ItemColor
        {
            get { return _itemColor; }
            set
            {
                _itemColor = value;
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

        private bool _editorLeftIsVisible;
        public bool EditorLeftIsReadOnly
        {
            get { return _editorLeftIsVisible; }
            set
            {
                _editorLeftIsVisible = value;
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
