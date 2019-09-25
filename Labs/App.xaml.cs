using System;
using System.Globalization;
using Labs.Helpers;
using Labs.Resources;
using Labs.Views;
using Plugin.Multilingual;
using Plugin.Settings;
using Xamarin.Forms;

namespace Labs
{
    public partial class App : Application
    {
        [Obsolete]
        public App()
        {
            InitializeComponent();
            SetCulture();
            //SetTheme();
            MainPage = new MainPage();
        }

        //private static volatile App _instance;
        //private static object syncRoot = new Object();

        //public static App Instance
        //{
        //    get
        //    {
        //        if (_instance == null)
        //        {
        //            lock (syncRoot)
        //            {
        //                if (_instance == null) {
        //                    _instance = new App();
        //                }
        //            }
        //        }

        //        return _instance;
        //    }
        //}

        protected override void OnStart()
        {
            SetCulture();
            SetTheme();
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
            var culture = CrossSettings.Current.GetValueOrDefault(Constants.Culture, null);
            if (culture != null) {
                var cultureInfo = new CultureInfo(culture);
                AppResources.Culture = cultureInfo;
                CrossMultilingual.Current.CurrentCultureInfo = cultureInfo;
            }
        }

        private void SetTheme()
        {
            int theme = CrossSettings.Current.GetValueOrDefault(Constants.Theme, 0);
            ThemeSettings.SetTheme(theme == 0 ? ThemeSettings.Theme.Light : ThemeSettings.Theme.Dark);
        }
    }
}
