using System.Collections.ObjectModel;
using Labs.Helpers;
using Labs.Models;
using Labs.Resources;

namespace Labs.ViewModels
{
    public static class MasterDetailViewModel
    {
        public static ObservableCollection<MasterDetailModel> GetDetailItems()
        {
            var detailItems = new ObservableCollection<MasterDetailModel>
            {
                new MasterDetailModel {
                    ImageSource = ThemeSettings.GetCurrentTheme ? "Home.png" : "HomeWhite.png",
                    Text = AppResources.HomeButton, LineIsVisible = false
                },
                new MasterDetailModel {
                    ImageSource = ThemeSettings.GetCurrentTheme ? "file.png" : "fileWhite.png",
                    Text = AppResources.CreateTestButton, LineIsVisible = false
                },
                new MasterDetailModel {
                    ImageSource = ThemeSettings.GetCurrentTheme ? "Settings.png" : "SettingsWhite.png",
                    Text = AppResources.SettingsButton, LineIsVisible = true
                }
            };

            return detailItems;
        }
    }
}
