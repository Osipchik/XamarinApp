using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using FFImageLoading;
using FFImageLoading.Forms.Platform;
using Xamarin.Forms;

namespace Labs.Droid
{
    [Activity(Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
          
            base.OnCreate(savedInstanceState);
            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(true);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            CachedImageRenderer.InitImageViewHandler();
            LoadApplication(new App());
            MessagingCenter.Subscribe<object>(this, "SetStatusBarColor", SetStatusBarColor);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void SetStatusBarColor(object sender)
        {
            var color = (Color) sender;
            int red = (int)(color.R * 255);
            int green = (int)(color.G * 255);
            int blue = (int)(color.B * 255);
            int alpha = (int)(color.A * 255);
            Window.SetStatusBarColor(Android.Graphics.Color.Argb(alpha, red, green, blue));
        }
    }
}