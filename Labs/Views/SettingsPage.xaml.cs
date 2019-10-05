using System;
using System.Collections.ObjectModel;
using System.Globalization;
using Labs.Helpers;
using Labs.Models;
using Labs.Resources;
using Labs.ViewModels;
using Plugin.Multilingual;
using Plugin.Settings;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Labs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public ObservableCollection<Language> Languages { get; }
        public SettingsPage()
        {
            InitializeComponent();

            Languages = Language.GetLanguageList();
            BindingContext = this;
            SetButton();
            SetIndex();
        }

        private void PickerLanguages_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var culture = new CultureInfo(Languages[PickerLanguages.SelectedIndex].ShortName);
            AppResources.Culture = culture;
            CrossMultilingual.Current.CurrentCultureInfo = culture;
            CrossSettings.Current.AddOrUpdateValue(Language.CultureSetting, culture.ToString());
            MessagingCenter.Send<Page>(this, MainPage.UploadMainPage);
        }

        private void SetIndex()
        {
            for (var i = 0; i < Languages.Count; i++) {
                if (AppResources.Culture.Name.Contains(Languages[i].ShortName)) {
                    PickerLanguages.SelectedIndex = i;
                    break;
                }
            }
        }

        private void ButtonLight_OnClicked(object sender, EventArgs e)
        {
            ThemeSettings.SetTheme(ThemeSettings.Theme.Dark);
            MessagingCenter.Send<Page>(this, MainPage.UploadMainPage);
            SetButton();
        }

        private void ButtonDark_OnClicked(object sender, EventArgs e)
        {
            ThemeSettings.SetTheme(ThemeSettings.Theme.Light);
            MessagingCenter.Send<Page>(this, MainPage.UploadMainPage);
            SetButton();
        }

        private void SetButton()
        {
            ButtonLight.IsVisible = ThemeSettings.GetCurrentTheme;
            ButtonDark.IsVisible = !ButtonLight.IsVisible;
        }
    }
}