using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Labs.Views;
using Xamarin.Forms;
using AbsoluteLayout = Xamarin.Forms.AbsoluteLayout;
using Application = Android.App.Application;

namespace Labs.Droid
{
    [Activity(Label = "Labs", Icon = "@mipmap/TestIcon", Theme = "@style/SplashTheme", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            await Task.Run(StartUp);
        }
        private void StartUp()
        {
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }

        public class SplashPage : ContentPage
        {
            Image Splash;
            public SplashPage()
            {
                NavigationPage.SetHasNavigationBar(this, false);

                Splash = new Image
                {
                    Source = "splash_logo.png",
                    WidthRequest = 300,
                    HeightRequest = 300
                };
                var StackLayout = new AbsoluteLayout();
                AbsoluteLayout.SetLayoutFlags(Splash,
                    AbsoluteLayoutFlags.PositionProportional);
                AbsoluteLayout.SetLayoutBounds(Splash,
                    new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
                StackLayout.Children.Add(Splash);
                this.BackgroundColor = Color.White;
                this.Content = StackLayout;
                Device.StartTimer(TimeSpan.FromSeconds(3), () =>
                {
                    dif();
                    Device.StartTimer(TimeSpan.FromSeconds(4), () =>
                    {
                        Navigation.PushAsync(new MainPage());
                        return false;
                    });
                    return false;
                });
            }

            async void dif()
            {
                await Splash.ScaleTo(1.5, 3000);
                await Splash.ScaleTo(0.9, 1500);
                await Splash.ScaleTo(1.7, 5000);
            }
        }
    }
}