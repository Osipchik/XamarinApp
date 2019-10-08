using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Labs.ViewModels;
using Labs.Views.Creators;
using Labs.Views.Popups;
using Labs.Views.TestPages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace Labs.Views
{
    [DesignTimeVisible(false)]
    public partial class MainPage
    {
        private enum PageIndex
        {
            Home,
            Creator,
            Settings
        }

        public const string UploadMainPage = "UploadTitles";

        public MainPage()
        {
            InitializeComponent();

            ShowHomePage();
            BindingContext = this;
            ListViewDetail.ItemsSource = MasterDetailViewModel.GetDetailItems();
            MessagingCenter.Subscribe<Page>(this, UploadMainPage, UploadTitles);
        }

        private void UploadTitles(object obj) => ListViewDetail.ItemsSource = MasterDetailViewModel.GetDetailItems();

        private void ShowHomePage()
        {
            Detail = new NavigationPage(new HomePage());
            IsPresented = false;
        }

        private async void ListViewDetail_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            switch (e.ItemIndex)
            {
                case (int)PageIndex.Home:
                    ShowHomePage();
                    break;
                case (int)PageIndex.Creator:
                    await PopupNavigation.Instance.PushAsync(new LoadingPopup());
                    await Device.InvokeOnMainThreadAsync(() => { Detail = new NavigationPage(new CreatorMenuPage()); });
                    IsPresented = false;
                    break;
                case (int)PageIndex.Settings:
                    await Device.InvokeOnMainThreadAsync(() => { Detail = new NavigationPage(new SettingsPage()); });
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