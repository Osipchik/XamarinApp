using Labs.ViewModels;
using Xamarin.Forms.Xaml;

namespace Labs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartTestPage
    {
        public StartViewModel StartViewModel { get; }
        public StartTestPage(string path)
        {
            InitializeComponent();

            StartViewModel = new StartViewModel(path, this, ChangeButton, StartButton);
            ChangeButton.BindingContext = StartViewModel;
            StartButton.BindingContext = StartViewModel;
            BindingContext = this;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            StartViewModel.StartPageCallBack();
        }
    }
}