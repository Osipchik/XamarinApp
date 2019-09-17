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
            set
            {
                _borderColor = value;
                OnPropertyChanged();
            }
        }

        public string ItemTextLeft { get; set; }
        public string ItemTextRight { get; set; }

        public bool IsRight { get; set; }

        public string RightString { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
