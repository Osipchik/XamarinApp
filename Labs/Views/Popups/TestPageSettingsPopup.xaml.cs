using System;
using Labs.Models;
using Labs.Resources;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms.Xaml;

namespace Labs.Views.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TestPageSettingsPopup
    {
        public TestPageSettingsPopup(SettingsModel settings)
        {
            InitializeComponent();
            SettingsLayout.BindingContext = settings;
            LabelTitle.Text = AppResources.Settings.ToUpper();
            LabelOk.Text = AppResources.Ok;
        }

        private async void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }
    }
}