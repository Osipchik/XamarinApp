using System.ComponentModel;
using Xamarin.Forms;
using System.Runtime.CompilerServices;
using Labs.Annotations;

namespace Labs.Models
{
    public class FrameModel : INotifyPropertyChanged
    {
        private Color _borderColor;
        public Color BorderColor
        {
            get => _borderColor;
            set {
                _borderColor = value;
                OnPropertyChanged();
            }
        }

        private string _itemTextLeft;
        public string ItemTextLeft
        {
            get => _itemTextLeft;
            set
            {
                if (string.IsNullOrEmpty(value)) return;
                _itemTextLeft = value;
                OnPropertyChanged();

            }
        }

        private string _itemTextRight;
        public string ItemTextRight
        {
            get => _itemTextRight;
            set
            {
                if (string.IsNullOrEmpty(value)) return;
                _itemTextRight = value;
                OnPropertyChanged();
            }
        }

        private bool _editorLeftIsReadOnly;
        public bool EditorLeftIsReadOnly
        {
            get => _editorLeftIsReadOnly;
            set
            {
                _editorLeftIsReadOnly = value;
                OnPropertyChanged();
            }
        }

        private bool _editorRightIsReadOnly;
        public bool EditorRightIsReadOnly
        {
            get => _editorRightIsReadOnly;
            set
            {
                _editorRightIsReadOnly = value;
                OnPropertyChanged();
            }
        }

        public bool IsRight { get; set; }

        private string _rightString;
        public string RightString
        {
            get => _rightString;
            set
            {
                _rightString = value;
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
