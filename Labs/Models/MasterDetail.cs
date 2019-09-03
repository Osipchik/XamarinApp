using System.Collections.ObjectModel;
using Labs.Resources;
using Xamarin.Forms;

namespace Labs.Models
{
    public class MasterDetail
    {
        public ImageSource Image { get; set; }
        public string Text { get; set; }
        public bool LineIsVisible { get; set; }

        public static ObservableCollection<MasterDetail> GetDetailItems()
        {
            var detailItems = new ObservableCollection<MasterDetail>
            {
                new MasterDetail{Image = "Home.png", Text = AppResources.HomeButton, LineIsVisible = false},
                new MasterDetail{Image = "file.png", Text = AppResources.CreateTestButton, LineIsVisible = false},
                new MasterDetail{Image = "Settings.png", Text = AppResources.SettingsButton, LineIsVisible = true}
            };

            return detailItems;
        }
    }
}
