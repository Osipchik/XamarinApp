using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Labs.Models;
using System.Windows.Input;
using Labs.Annotations;
using Labs.Data;
using Labs.Helpers;
using Labs.Views;
using Realms;
using Xamarin.Forms;

namespace Labs.ViewModels
{
    class HomeViewModel : Search, INotifyPropertyChanged
    {
        private static IList<Label> _labels;

        public HomeViewModel(Grid grid, params Label[] labels)
        {
            Grid = grid;
            _labels = labels;
            Filter = FilterMode.Name;
            SetCommands();
        }

        public INavigation Navigation { get; set; }

        private ObservableCollection<TestInfoModel> _testModels = new ObservableCollection<TestInfoModel>();
        public ObservableCollection<TestInfoModel> GetInfo
        {
            get => _testModels;
            private set
            {
                _testModels = value;
                OnPropertyChanged();
            }
        }

        private string _searchBarText;
        public string SearchBarText
        {
            get => _searchBarText;
            set
            {
                _searchBarText = value;
                GetInfo = StartSearch(SearchBarText).ToObservableCollection();
            }
        }

        public ICommand NameLabelTapCommand { get; set; }
        public ICommand DateLabelTapCommand { get; set; }
        public ICommand SubjectLabelTapCommand { get; set; }

        private void SetCommands()
        {
            NameLabelTapCommand = new Command(() => { SetLabelTapCommand(_labels[0], FilterMode.Name); });
            SubjectLabelTapCommand = new Command(() => { SetLabelTapCommand(_labels[1], FilterMode.Subject); });
            DateLabelTapCommand = new Command(() => { SetLabelTapCommand(_labels[2], FilterMode.Date); });
        }

        public async void SetInfoAsync() =>
            await Task.Run(() => {
                using (var realm = Realm.GetInstance())
                {
                    GetInfo.Clear();
                    foreach (var query in realm.All<TestModel>().Where(d => !d.IsTemp)) {
                        GetInfo.Add(new TestInfoModel
                        {
                            TestId = query.Id,
                            Name = query.Name,
                            Subject = query.Subject,
                            Date = query.Date.ToString("d"),
                        });
                    }

                    TestModels = GetInfo;
                }
            });

        public async void GoToStartTestPage(int index) =>
            await Navigation.PushAsync(new StartTestPage(GetInfo[index].TestId));

        private void SetLabelTapCommand(Label sender, FilterMode filter)
        {
            ActiveSearchLabelStyle(sender);
            DisableSearchLabelStyle(sender);
            Filter = filter;
            GetInfo = StartSearch(SearchBarText).ToObservableCollection();
        }

        private static void ActiveSearchLabelStyle(Label label)
        {
            label.FontSize = 16;
            label.TextColor = GetColor(true);
        }

        private static void DisableSearchLabelStyle(Label activeLabel)
        {
            foreach (var label in _labels.Where(label => activeLabel != label)) {
                label.FontSize = 14;
                label.TextColor = GetColor(false);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
