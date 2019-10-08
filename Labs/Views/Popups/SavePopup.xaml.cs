using System;
using Labs.Resources;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms.Xaml;

namespace Labs.Views.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SavePopup
    {
        public SavePopup()
        {
            InitializeComponent();
            LabelTitle.Text = AppResources.Save.ToUpper();
            LabelCancel.Text = AppResources.Cancel.ToUpper();
            animationView.OnFinish += TapGestureRecognizer_OnTapped;
        }

        private async void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }
    }
}