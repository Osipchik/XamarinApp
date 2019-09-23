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

        public static async void SetTheme(Theme theme)
        {
            await Device.InvokeOnMainThreadAsync(() =>
            {
                // TODO: fix resources list
                var asd = Application.Current.Resources.MergedDictionaries;
                switch (theme)
                {
                    case Theme.Dark:
                        CrossSettings.Current.AddOrUpdateValue(Constants.Theme, (int) Theme.Dark);
                        Application.Current.Resources.Add(new DarkTheme());
                        break;
                    case Theme.Light:
                        CrossSettings.Current.AddOrUpdateValue(Constants.Theme, (int)Theme.Light);
                        Application.Current.Resources.Add(new LightTheme());
                        break;
                    default:
                        break;
                }
            });
        }
    }
}
