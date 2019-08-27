using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using Labs.MainPages;
using Labs.Models;
using Labs.Resources;
using Xamarin.Forms;

namespace Labs.Views
{
    [DesignTimeVisible(false)]
    public partial class MainPage
    {
        private static readonly string FolderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        private readonly DirectoryInfo _dirInfoTests = new DirectoryInfo(Path.Combine(FolderPath, "Tests"));

        private ObservableCollection<MasterDetail> _detailItems;

        [Obsolete]
        public MainPage()
        {
            InitializeComponent();

            IsPresented = false;
            if (!_dirInfoTests.Exists) _dirInfoTests.Create();
            Detail = new NavigationPage(new HomePage());

            _detailItems = new ObservableCollection<MasterDetail>
            {
                new MasterDetail{Image = "Home.png", Text = AppResources.HomeButton, LineIsVisible = false},
                new MasterDetail{Image = "file.png", Text = AppResources.CreateTestButton, LineIsVisible = false},
                new MasterDetail{Image = "Settings.png", Text = AppResources.SettingsButton, LineIsVisible = true}
            };
            BindingContext = this;
            ListViewDetail.ItemsSource = _detailItems;

            Subscribe();
        }

        private void Subscribe()
        {
            MessagingCenter.Subscribe<Page>(
                this,
                "UploadTitles",
                (sender) => UploadTitles());
        }

        private void UploadTitles()
        {
            _detailItems[0].Text = AppResources.HomeButton;
            _detailItems[1].Text = AppResources.CreateTestButton;
            _detailItems[2].Text = AppResources.SettingsButton;

            ListViewDetail.ItemsSource = null;
            ListViewDetail.ItemsSource = _detailItems;
        }

        [Obsolete]
        private void ListViewDetail_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            switch (e.ItemIndex)
            {
                case 0:
                    Detail = new NavigationPage(new HomePage());
                    IsPresented = false;
                    break;
                case 1:
                    Detail = new NavigationPage(new CreatorPage("Temp"));
                    IsPresented = false;
                    break;
                case 2:
                    Detail = new NavigationPage(new SettingsPage());
                    IsPresented = false;
                    break;
            }
        }

        private void ListViewDetail_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = (ListView)sender;
            item.SelectedItem = null;
        }
    }
}