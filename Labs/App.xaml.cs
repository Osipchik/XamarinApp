using System.Globalization;
using Labs.Helpers;
using Labs.Models;
using Labs.Resources;
using Labs.Views;
using Plugin.Multilingual;
using Plugin.Settings;
using Xamarin.Forms;

namespace Labs
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            SetCulture();
            SetTheme();
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            //SetCulture();
            //SetTheme();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            SetCulture();
            SetTheme();
        }

        private void SetCulture()
        {
            var culture = CrossSettings.Current.GetValueOrDefault(Language.CultureSetting, null);
            if (culture != null) {
                var cultureInfo = new CultureInfo(culture);
                AppResources.Culture = cultureInfo;
                CrossMultilingual.Current.CurrentCultureInfo = cultureInfo;
            }
            else
            {
                AppResources.Culture = CultureInfo.CurrentCulture;
                CrossMultilingual.Current.CurrentCultureInfo = CultureInfo.CurrentCulture;
            }
        }

        private void SetTheme()
        {
            int theme = CrossSettings.Current.GetValueOrDefault(ThemeSettings.ThemeSetting, 0);
            ThemeSettings.SetTheme(theme == 0 ? ThemeSettings.Theme.Light : ThemeSettings.Theme.Dark);
        }
    }
}
