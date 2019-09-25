using System.Threading.Tasks;
using Xamarin.Forms;

namespace Labs.Helpers
{
    public static class FrameAnimation
    {
        public static void RunShowOrHideAnimation(View view, uint heightMax, uint heightMin, bool show, bool isVisible = true) =>
            RunSettingsViewAnimationAsync(view, show ? heightMax : heightMin, isVisible); 

        public static void RunShowOrHideButtonAnimation(Button button, uint widthMax, bool show)
        {
            ChangeButtonStyle(button, show);
            if (show) RunSettingsButtonAnimationToShowAsync(button, widthMax);
            else RunSettingsButtonAnimationToHideAsync(button);
        }

        private static async void RunSettingsButtonAnimationToShowAsync(Button button, uint widthMax)
        {
            await Task.Run(() => {
                new Animation((d) => button.WidthRequest = d, button.Width, widthMax)
                    .Commit(button, "ButtonShow", Constants.AnimationRate, Constants.AnimationLength, Easing.SinInOut);
            });
        }
        private static async void RunSettingsButtonAnimationToHideAsync(Button button)
        {
            var buttonWidthMin = button.Text.Length * Constants.SymbolLength;
            await Task.Run(() => {
                new Animation((d) => button.WidthRequest = d, button.Width, buttonWidthMin)
                    .Commit(button, "ButtonHide", Constants.AnimationRate, Constants.AnimationLength, Easing.SinInOut);
            });
        }

        private static async void RunSettingsViewAnimationAsync(View view, uint heightEnd, bool isVisible = true)
        {
            await Task.Run(() => {
                new Animation((d) => view.HeightRequest = d, view.Height, heightEnd)
                    .Commit(view, "ShowOrShow", Constants.AnimationRate, Constants.AnimationLength, Easing.Linear);
                if (isVisible) {
                    Device.InvokeOnMainThreadAsync(() => view.IsVisible = !view.IsVisible);
                }
            });
        }

        private static void ChangeButtonStyle(Button button, bool show = true)
        {
            if (show) ButtonStyleShow(button);
            else ButtonStyleHide(button);
        }

        private static void ButtonStyleShow(Button button)
        {
            button.BackgroundColor = (Color) Application.Current.Resources["ColorMaterialBlue"];
            button.TextColor = Color.White;
        }

        private static void ButtonStyleHide(Button button)
        {
            button.BackgroundColor = Color.White;
            button.TextColor = (Color)Application.Current.Resources["ColorMaterialBlue"];
        }
    }
}