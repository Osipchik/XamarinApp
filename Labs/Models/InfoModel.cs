using System.ComponentModel;
using System.Runtime.CompilerServices;
using Labs.Annotations;
using Xamarin.Forms;

namespace Labs.Models
{
    public class InfoModel : INotifyPropertyChanged
    {
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

        private string _detail;
        public string Detail
        {
            get => _detail;
            set
            {
                _detail = value;
                OnPropertyChanged();
            }
        }

        private string _date;
        public string Date
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged();
            }
        }

        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        private Color _titleColor;
        public Color TitleColor
        {
            get => _titleColor;
            set
            {
                _titleColor = value;
                OnPropertyChanged();
            }
        }

        private Color _detailColor;
        public Color DetailColor
        {
            get => _detailColor;
            set
            {
                _detailColor = value;
                OnPropertyChanged();
            }
        }

        private Color _dateColor;
        public Color DateColor
        {
            get => _dateColor;
            set
            {
                _dateColor = value;
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
