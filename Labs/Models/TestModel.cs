using System.ComponentModel;
using System.Runtime.CompilerServices;
using Labs.Annotations;

namespace Labs.Models
{
    public class TestModel : INotifyPropertyChanged
    {
        private int _price;
        public int Price
        {
            get => _price;
            set
            {
                _price = value;
                OnPropertyChanged();
            }
        }

        private int _rightAnswers;
        public int RightAnswers
        {
            get => _rightAnswers;
            set
            {
                _rightAnswers = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
