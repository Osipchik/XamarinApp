using System;
using Labs.Resources;
using Labs.Views;
using Plugin.Multilingual;
using Xamarin.Forms;

namespace Labs
{
    public partial class App : Application
    {
        [Obsolete]
        public App()
        {
            InitializeComponent();
            
            var culture = CrossMultilingual.Current.DeviceCultureInfo;
            AppResources.Culture = culture;

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
