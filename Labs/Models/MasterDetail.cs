using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Labs.Annotations;
using Labs.Resources;
using Xamarin.Forms;

namespace Labs.Models
{
    public class MasterDetail : INotifyPropertyChanged
    {
        private ImageSource _imageSource;
        public ImageSource ImageSource
        {
            get { return _imageSource;}
            set
            {
                _imageSource = value;
                OnPropertyChanged();
            }
        }

        private string _text;
        public string Text
        {
            get { return _text;}
            set
            {
                _text = value;
                OnPropertyChanged();
            }
        }

        public bool LineIsVisible { get; set; }

        public static ObservableCollection<MasterDetail> GetDetailItems()
        {
            var detailItems = new ObservableCollection<MasterDetail>
            {
                new MasterDetail{ImageSource = "Home.png", Text = AppResources.HomeButton, LineIsVisible = false},
                new MasterDetail{ImageSource = "file.png", Text = AppResources.CreateTestButton, LineIsVisible = false},
                new MasterDetail{ImageSource = "Settings.png", Text = AppResources.SettingsButton, LineIsVisible = true}
            };

            return detailItems;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
