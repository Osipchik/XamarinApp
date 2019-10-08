using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
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
    }
}