using Labs.Helpers;
using Labs.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Labs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage
    {
        private readonly HomeViewModel _homeViewModel;

        public HomePage()
        {
            InitializeComponent();

            _homeViewModel = new HomeViewModel(GridButtons, LabelName, LabelSubject, LabelDate);
            listView.BindingContext = _homeViewModel.InfoViewModel;
            BindingContext = _homeViewModel;
            Subscribe();
        }

        private void Subscribe()
        {
            MessagingCenter.Subscribe<Page>(this, Constants.HomeListUpload, 
                (sender) => { _homeViewModel.RefreshModels(); });
        }
        
        private void ListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            _homeViewModel.GoToStartTestPage(e.ItemIndex, this);
        }
        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }
    }
}