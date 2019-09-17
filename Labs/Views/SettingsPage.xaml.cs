using System;
using System.Collections.ObjectModel;
using System.Globalization;
using Labs.Helpers;
using Labs.Models;
using Labs.Resources;
using Plugin.Multilingual;
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
            PickerLanguages.SelectedIndexChanged += PickerLanguagesOnSelectedIndexChanged;
            SetIndex();
        }

        private void PickerLanguagesOnSelectedIndexChanged(object sender, EventArgs e)
        {
            var language = Languages[PickerLanguages.SelectedIndex];
            var culture = new CultureInfo(language.ShortName);
            AppResources.Culture = culture;
            CrossMultilingual.Current.CurrentCultureInfo = culture;

            LabelLanguage.Text = AppResources.Labguage;
            LabelTheme.Text = AppResources.Theme;

            SetPickerWidth();

            MessagingCenter.Send<Page>(this, Constants.UploadTitles);
        }

        private void SetIndex()
        {
            for (var i = 0; i < Languages.Count; i++) {
                if (AppResources.Culture.Equals(new CultureInfo(Languages[i].ShortName))) {
                    PickerLanguages.SelectedIndex = i;
                    break;
                }
            }
        }

        private void SetPickerWidth()
        {
            var width = 0;
            if (AppResources.Culture.Equals(new CultureInfo("ru-RU"))) {
                width = 155;
            }
            else if (AppResources.Culture.Equals(new CultureInfo("de-DE"))) {
                width = 160;
            }
            else if (AppResources.Culture.Equals(new CultureInfo("es-ES"))) {
                width = 150;
            }
            else if (AppResources.Culture.Equals(new CultureInfo("fr"))) {
                width = 144;
            }
            else if (AppResources.Culture.Equals(new CultureInfo("it"))) {
                width = 130;
            }
            else if (AppResources.Culture.Equals(new CultureInfo("ja"))) {
                width = 155;
            }
            else if (AppResources.Culture.Equals(new CultureInfo("ko"))) {
                width = 127;
            }
            else if (AppResources.Culture.Equals(new CultureInfo("zh-Hans"))) {
                width = 215;
            }
            else if (AppResources.Culture.Equals(new CultureInfo("zh-Hant"))) {
                width = 170;
            }
            else if (AppResources.Culture.Equals(new CultureInfo("en"))) {
                width = 65;
            }

            PickerLanguages.WidthRequest = width;
        }
    }
}