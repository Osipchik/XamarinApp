using System.Threading.Tasks;
using Labs.Interfaces;
using Labs.ViewModels;
using Labs.ViewModels.Tests;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Labs.Views.TestPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntryTypeTestPage : ContentPage
    {
        private readonly TestPageViewModel _viewModelTest;
        public EntryTypeTestPage(string questionId, TimerViewModel testTimeViewModel, string time, ISettings settings, int index)
        {
            _viewModelTest = new TestPageViewModel(questionId, testTimeViewModel, time, settings, index);
            InitializeComponent();
            Title = (1 + index).ToString();
            BindingContext = _viewModelTest;
            Subscribe(index);
        }

        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e) =>
            ((ListView)sender).SelectedItem = null;

        protected sealed override void OnAppearing()
        {
            base.OnAppearing();
            _viewModelTest.OnAppearing(this);
        }

        private void Subscribe(int? num)
        {
            if (num.HasValue && num.Value == 1) {
                MessagingCenter.Subscribe<Page>(this, TestPageViewModel.RunFirstTimer,
                    (sender) => { _viewModelTest.OnAppearing(this); });
            }
            MessagingCenter.Subscribe<Page>(this, ResultPage.Check,
                (sender) => { _viewModelTest.SetResult(CheckModel()); });
        }

        private bool CheckModel()
        {
            var isRight = _viewModelTest.GetFrameModel[0].Text == _viewModelTest.GetFrameModel[0].MainText;
            _viewModelTest.GetFrameModel[0].BorderColor = FrameViewModel.GetColorOnCheck(isRight);

            return isRight;
        }
    }
}