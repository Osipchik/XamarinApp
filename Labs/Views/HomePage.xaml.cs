using Labs.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Labs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage
    {
        private readonly HomeViewModel _homeViewModel;
        private Frame _tappedFrame;
        
        public HomePage()
        {
            InitializeComponent();

            _homeViewModel = new HomeViewModel(GridButtons, LabelName, LabelSubject, LabelDate);

            //ListUploadAsync();
            //Subscribe();

            listView.BindingContext = _homeViewModel.InfoViewModel;
            BindingContext = _homeViewModel;
        }

        //private void ListUploadAsync() => listView.ItemsSource = _homeViewModel.GetModels.GetDirectoryInfo();
        
        //private void Subscribe() =>
        //    MessagingCenter.Subscribe<Page>(this, Constants.HomeListUpload, (sender) =>
        //    {
        //        ListUploadAsync();
        //    });


        private async void ListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            await Navigation.PushAsync(new StartTestPage(_homeViewModel.InfoViewModel.GetElementPath(e.ItemIndex)));
        }

        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private void TapEvent() => TapViewModel.GetTapGestureRecognizer(_tappedFrame);
    }
}