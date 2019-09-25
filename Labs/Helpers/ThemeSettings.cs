using Labs.Theme;
using Plugin.Settings;
using Xamarin.Forms;

namespace Labs.Helpers
{
    public static class ThemeSettings
    {
        public enum Theme
        {
            Light,
            Dark
        }

        public static bool GetCurrentTheme { private set; get; }
        public static void SetTheme(Theme theme)
        {
            switch (theme)
            {
                case Theme.Dark:
                    GetCurrentTheme = false;
                    CrossSettings.Current.AddOrUpdateValue(Constants.Theme, (int)Theme.Dark);
                    Application.Current.Resources.Add(new DarkTheme());
                    break;
                case Theme.Light:
                    GetCurrentTheme = true;
                    CrossSettings.Current.AddOrUpdateValue(Constants.Theme, (int)Theme.Light);
                    Application.Current.Resources.Add(new LightTheme());
                    break;
                default:
                    break;
            }
        }
    }
}
