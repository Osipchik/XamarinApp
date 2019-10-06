using System;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms.Xaml;


namespace Labs.Views.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WarningPopup
    {
        public WarningPopup(string title, string text, string cancelButton)
        {
            InitializeComponent();
            LabelTitle.Text = title.ToUpper();
            LabelText.Text = text;
            LabelCancel.Text = cancelButton.ToUpper();
        }

        private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }
    }
}