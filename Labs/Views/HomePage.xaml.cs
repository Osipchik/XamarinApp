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
        private static bool _isClickAble = true;

        public HomePage()
        {
            InitializeComponent();

            _homeViewModel = new HomeViewModel(GridButtons, LabelName, LabelSubject, LabelDate);
            BindingContext = _homeViewModel;
        }

        private void ListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (_isClickAble) {
                _isClickAble = false;
                _homeViewModel.GoToStartTestPage(e.ItemIndex, this);
            }
        }

        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e) =>
            ((ListView)sender).SelectedItem = null;

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _homeViewModel.RefreshModels();
            _isClickAble = true;
        }
    }
}