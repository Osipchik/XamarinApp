using System;
using System.ComponentModel;
using Labs.ViewModels;
using Labs.Views.Creators;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Labs.Views
{
    [DesignTimeVisible(false)]
    public partial class MainPage
    {
        public const string UploadMainPage = "UploadTitles";

        public MainPage()
        {
            InitializeComponent();

            ShowHomePage();
            BindingContext = this;
            ListViewDetail.ItemsSource = MasterDetailViewModel.GetDetailItems();
            Subscribe();
        }

        private void Subscribe() => MessagingCenter.Subscribe<Page>(this, UploadMainPage, UploadTitles);

        private void UploadTitles(object obj) => ListViewDetail.ItemsSource = MasterDetailViewModel.GetDetailItems();

        private void ShowHomePage()
        {
            Detail = new NavigationPage(new HomePage());
            IsPresented = false;
        }

        [Obsolete]
        private async void ListViewDetail_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            switch (e.ItemIndex)
            {
                case 0:
                    ShowHomePage();
                    break;
                case 1:
                    await Device.InvokeOnMainThreadAsync(() => {
                        Detail = new NavigationPage(new CreatorMenuPage());
                    });
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