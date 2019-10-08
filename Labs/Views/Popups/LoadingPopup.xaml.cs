using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Labs.Views.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingPopup
    {
        public const string Finish = "finish";

        public LoadingPopup()
        {
            InitializeComponent();
            MessagingCenter.Subscribe<Page>(this, Finish, 
                async (sender) => PushBack());
        }

        private async void PushBack()
        {
            await PopupNavigation.Instance.PopAsync();
            MessagingCenter.Unsubscribe<Page>(this, Finish);
        }

        protected override bool OnBackgroundClicked()
        {
            return false;
        }
    }
}