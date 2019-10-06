using Labs.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Labs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartTestPage
    {
        private readonly string _testId;
        public StartViewModel StartViewModel { get; private set; }

        public StartTestPage(string testId)
        {
            InitializeComponent();
            _testId = testId;
            Initialize();
        }

        private void Initialize()
        {
            StartViewModel = new StartViewModel(_testId)
            {
                ChangeButton = ChangeButton, StartButton = StartButton, Navigation = Navigation
            };
            Device.BeginInvokeOnMainThread(()=> BindingContext = StartViewModel);
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            StartViewModel.StartPageCallBack();
        }
    }
}