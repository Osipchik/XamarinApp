using System;
using System.Collections.Generic;
using System.IO;
using Labs.Helpers;
using Labs.Models;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Labs.ViewModels
{
    class HomeViewModel
    {
        private readonly Grid _grid;
        private readonly Label[] _labels;
        public HomeViewModel(Grid grid, params Label[] labels)
        {
            GetModels = new InfoViewModel(GetMainTestFolderPath());
            _grid = grid;
            _labels = labels;
        }

        public InfoViewModel GetModels { get; }

        private string GetMainTestFolderPath()
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return Path.Combine(folder, Constants.TestFolder);
        }

        private IEnumerable<InfoModel> SearchByFilter(string keyword, string filter)
        {
            IEnumerable<InfoModel> searchQuery = from model in GetModels.InfosModel
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

        public IEnumerable<InfoModel> Search(string keyword)
        {
            IEnumerable<InfoModel> models;
            if (string.IsNullOrEmpty(keyword)) {
                _grid.IsVisible = false;
                DisableSearchModificationAsync();
                models = GetModels.InfosModel;
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
                if (_labels[i].TextColor == Constants.ColorMaterialBlue) {
                    filter = GetModels.GetNameOfPropertyInModel(i);
                    break;
                }
            }

            return SearchByFilter(keyword, filter);
        }

        public void ActiveSearchLabelStyle(Label label)
        {
            label.FontSize = 16;
            label.TextColor = Constants.ColorMaterialBlue;
        }
        
        public void DisableSearchLabelStyle(Label activeLabel)
        {
            foreach (var label in _labels.Where(label => activeLabel != label)) {
                label.FontSize = 14;
                label.TextColor = Constants.ColorMaterialBlueGray;
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
            var color = filter == propertyName ? Constants.ColorMaterialBlue : Constants.ColorMaterialBlueGray;
            return color;
        }

        private async void DisableSearchModificationAsync()
        {
            await Task.Run(() => {
                foreach (var model in GetModels.InfosModel) {
                    model.TitleColor =
                        model.DetailColor =
                            model.DateColor = Constants.ColorMaterialBlueGray;
                }
            });
        }
    }
}
