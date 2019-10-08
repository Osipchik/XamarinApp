using System.Threading.Tasks;
using Labs.Interfaces;
using Labs.ViewModels;
using Labs.ViewModels.Tests;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Labs.Views.TestPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CheckTypeTestPage : ContentPage
    {
        private readonly TestPageViewModel _viewModelTest;

        public CheckTypeTestPage(string questionId, TimerViewModel testTimeViewModel, string time, ISettings settings, int index)
        {
            InitializeComponent();
            _viewModelTest = new TestPageViewModel(questionId, testTimeViewModel, time, settings, index);
            BindingContext = _viewModelTest;
            Title = (1+index).ToString();
            Subscribe(index);
        }

        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e) => 
            ((ListView)sender).SelectedItem = null;

        private void ListView_OnItemTapped(object sender, ItemTappedEventArgs e) => _viewModelTest.TapEvent(e.ItemIndex);
        
        protected sealed override void OnAppearing()
        {
            base.OnAppearing();
            _viewModelTest.OnAppearing(this);
        }

        private void Subscribe(int? num)
        {
            if (num.HasValue && num.Value == 0) {
                MessagingCenter.Subscribe<Page>(this, TestPageViewModel.RunFirstTimer,
                    (sender) => { _viewModelTest.OnAppearing(this); });
            }
            MessagingCenter.Subscribe<Page>(this, ResultPage.Check,
                (sender) => { _viewModelTest.SetResult(CheckPageAsync()); });
        }

        private bool CheckPageAsync()
        {
            var pageIsRight = true;
            foreach (var model in _viewModelTest.GetFrameModel)
            {
                var isRight = model.BorderColor == (Color)Application.Current.Resources["ColorMaterialBlue"] ==
                              model.IsRight;
                model.BorderColor = FrameViewModel.GetColorOnCheck(isRight);
                pageIsRight = pageIsRight && isRight;
            }

            return pageIsRight;
        }
    }
}