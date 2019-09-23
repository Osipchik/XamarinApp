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
            SetIndex();
        }

        private void PickerLanguages_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var culture = new CultureInfo(Languages[PickerLanguages.SelectedIndex].ShortName);
            AppResources.Culture = culture;
            CrossMultilingual.Current.CurrentCultureInfo = culture;

            MessagingCenter.Send<Page>(this, (string) Application.Current.Resources["UploadTitles"]);
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
    }
}