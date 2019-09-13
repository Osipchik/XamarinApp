using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using Labs.Helpers;
using Labs.Models;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Labs.Annotations;
using Labs.Views;
using Xamarin.Forms;

namespace Labs.ViewModels
{
    class HomeViewModel : INotifyPropertyChanged
    {
        private readonly Grid _grid;
        private readonly Label[] _labels;
        public readonly InfoViewModel InfoViewModel;
        private IEnumerable<InfoModel> _infoModels;

        private string _searchBarText;
        public string SearchBarText
        {
            get => _searchBarText;
            set
            {
                _searchBarText = value;
                InfoViewModel.InfoModels = Search(SearchBarText).ToObservableCollection();
                OnPropertyChanged();
            }
        }

        public HomeViewModel(Grid grid, params Label[] labels)
        {
            _grid = grid;
            _labels = labels;
            InfoViewModel = new InfoViewModel(GetMainTestFolderPath());
            RefreshModels();
            SetCommands();
        }

        public void RefreshModels()
        {
            InfoViewModel.SetDirectoriesInfoAsync();
            _infoModels = InfoViewModel.InfoModels;
        }

        public ICommand NameLabelTapCommand { protected set; get; }
        public ICommand SubjectLabelTapCommand { protected set; get; }
        public ICommand DateLabelTapCommand { protected set; get; }

        private void SetCommands()
        {
            NameLabelTapCommand = new Command(() => { SetLabelTapCommand(0); });
            SubjectLabelTapCommand = new Command(() => { SetLabelTapCommand(1); });
            DateLabelTapCommand = new Command(() => { SetLabelTapCommand(2); });
        }
        private void SetLabelTapCommand(int index)
        {
            ActiveSearchLabelStyle(_labels[index]);
            DisableSearchLabelStyle(_labels[index]);
            InfoViewModel.InfoModels = Search(SearchBarText).ToList().ToObservableCollection();
        }

        private string GetMainTestFolderPath()
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return Path.Combine(folder, Constants.TestFolder);
        }

        private IEnumerable<InfoModel> Search(string keyword)
        {
            IEnumerable<InfoModel> models;
            if (string.IsNullOrEmpty(keyword)) {
                _grid.IsVisible = false;
                DisableSearchModificationAsync();
                models = _infoModels;
            }
            else {
                _grid.IsVisible = true;
                models = StartSearch(keyword);
            }

            return models;
        }
        private IEnumerable<InfoModel> StartSearch(string keyword)
        {
            var filter = string.Empty;
            for (var i = 0; i < _labels.Length; i++) {
                if (_labels[i].TextColor == Constants.Colors.ColorMaterialBlue) {
                    filter = InfoViewModel.GetNameOfPropertyInModel(i);
                    break;
                }
            }

            return SearchByFilter(keyword, filter);
        }
        private IEnumerable<InfoModel> SearchByFilter(string keyword, string filter)
        {
            IEnumerable<InfoModel> searchQuery = from model in _infoModels
                                                 where FilterBy(model, keyword, filter)
                                                 select model;
            return searchQuery;
        }
        private bool FilterBy(InfoModel model, string keyword, string filter)
        {
            var itContain = false;
            switch (filter)
            {
                case nameof(model.Name):
                    itContain = model.Title.ToLower().Contains(keyword.ToLower());
                    break;
                case nameof(model.Detail):
                    itContain = model.Detail.ToLower().Contains(keyword.ToLower());
                    break;
                case nameof(model.Date):
                    itContain = model.Date.ToLower().Contains(keyword.ToLower());
                    break;
            }
            if (itContain) ChangeStyleOfLabelsAsync(model, filter);

            return itContain;
        }

        private void ActiveSearchLabelStyle(Label label)
        {
            label.FontSize = 16;
            label.TextColor = Constants.Colors.ColorMaterialBlue;
        }
        private void DisableSearchLabelStyle(Label activeLabel)
        {
            foreach (var label in _labels.Where(label => activeLabel != label)) {
                label.FontSize = 14;
                label.TextColor = Constants.Colors.ColorMaterialGray;
            }
        }
        private async void ChangeStyleOfLabelsAsync(InfoModel model, string filter)
        {
            await Task.Run(() =>
            {
                model.TitleColor = GetColor(filter, nameof(model.Name));
                model.DetailColor = GetColor(filter, nameof(model.Detail));
                model.DateColor = GetColor(filter, nameof(model.Date));
            });
        }
        private Color GetColor(string filter, string propertyName)
        {
            return filter == propertyName ? Constants.Colors.ColorMaterialBlue : Constants.Colors.ColorMaterialGrayText;
        }

        private async void DisableSearchModificationAsync()
        {
            await Task.Run(() => {
                foreach (var model in _infoModels) {
                    model.TitleColor =
                        model.DetailColor =
                            model.DateColor = Constants.Colors.ColorMaterialGrayText;
                }
            });
        }

        public async void GoToStartTestPage(int index, Page page)
        {
            await page.Navigation.PushAsync(new StartTestPage(InfoViewModel.GetElementPath(index)));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public static class Extensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> collection)
        {
            return new ObservableCollection<T>(collection);
        }
    }
}
