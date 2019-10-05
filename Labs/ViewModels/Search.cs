using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Labs.Models;
using Xamarin.Forms;

namespace Labs.ViewModels
{
    public abstract class Search
    {
        protected enum FilterMode
        {
            Name,
            Subject,
            Date
        }

        protected Grid Grid;
        protected static FilterMode Filter;
        protected IEnumerable<TestInfoModel> TestModels { get; set; }

        protected IEnumerable<TestInfoModel> StartSearch(string keyword)
        {
            IEnumerable<TestInfoModel> models;
            DisableSearchModificationAsync();
            if (string.IsNullOrEmpty(keyword)) {
                Device.BeginInvokeOnMainThread(() => Grid.IsVisible = false);
                models = TestModels;
            }
            else {
                Device.BeginInvokeOnMainThread(() => Grid.IsVisible = true);
                models = SearchByFilter(keyword);
            }

            return models;
        }

        private IEnumerable<TestInfoModel> SearchByFilter(string keyword)
        {
            var searchQuery = from model in TestModels
                              where FilterBy(model, keyword, Filter)
                              select model;
            return searchQuery;
        }

        private static bool FilterBy(TestInfoModel model, string keyword, FilterMode filter)
        {
            var itContain = false;
            switch (filter)
            {
                case FilterMode.Name:
                    itContain = model.Name.ToLower().Contains(keyword.ToLower());
                    break;
                case FilterMode.Subject:
                    itContain = model.Subject.ToLower().Contains(keyword.ToLower());
                    break;
                case FilterMode.Date:
                    itContain = model.Date.ToLower().Contains(keyword.ToLower());
                    break;
            }

            ChangeStyleOfLabelsAsync(model, itContain);

            return itContain;
        }

        private static async void ChangeStyleOfLabelsAsync(TestInfoModel model, bool change)
        {
            if (change) {
                await Task.Run(() => {
                    model.LabelNameColor = GetColor(Filter == FilterMode.Name);
                    model.LabelDateColor = GetColor(Filter == FilterMode.Date);
                    model.LabelSubjectColor = GetColor(Filter == FilterMode.Subject);
                });
            }
        }
        
        protected static Color GetColor(bool isRight) => isRight 
            ? (Color)Application.Current.Resources["ColorMaterialBlue"]
            : (Color)Application.Current.Resources["TextColor"];

        private async void DisableSearchModificationAsync() => 
            await Task.Run(() => {
                foreach (var model in TestModels) {
                    model.LabelNameColor = GetColor(false);
                    model.LabelSubjectColor = GetColor(false);
                    model.LabelDateColor = GetColor(false);
                }
            });
    }
}
