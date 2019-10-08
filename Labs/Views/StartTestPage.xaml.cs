using System.Threading.Tasks;
using Labs.ViewModels;
using Labs.Views.TestPages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Labs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartTestPage
    {
        public StartViewModel StartViewModel { get; private set; }

        public StartTestPage(string testId)
        {
            InitializeComponent();
            StartViewModel = new StartViewModel(testId)
            {
                ChangeButton = ChangeButton,
                StartButton = StartButton,
                Navigation = Navigation
            };
            BindingContext = StartViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            StartViewModel.StartPageCallBack();
        }
    }
}