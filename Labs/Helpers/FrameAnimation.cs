using System.Threading.Tasks;
using Xamarin.Forms;

namespace Labs.Helpers
{
    public class FrameAnimation
    {
        private readonly uint _buttonWidthMax, _settingsViewHeightMax, _settingsViewHeightMin;

        public FrameAnimation(uint buttonWidthMax, uint viewHeightMax, uint viewHeightMin)
        {
            _buttonWidthMax = buttonWidthMax;
            _settingsViewHeightMax = viewHeightMax;
            _settingsViewHeightMin = viewHeightMin;
        }

        public void RunShowOrHideAnimation(Button button, View view, IAnimatable owner, bool show)
        {
            if (show) RunShowAnimation(button, view, owner);
            else RunHideAnimation(button, view, owner);
        }

        private void RunShowAnimation(Button button, View view, IAnimatable owner)
        {
            if (button != null) {
                ChangeButtonStyle(button);
                RunSettingsButtonAnimationToShowAsync(button, owner);
            }
            RunSettingsViewAnimationToShowAsync(view, owner);
        }
        private void RunHideAnimation(Button button, View view, IAnimatable owner)
        {
            if (button != null) {
                ChangeButtonStyle(button, false);
                RunSettingsButtonAnimationToHideAsync(button, owner);
            }
            RunSettingsViewAnimationToHideAsync(view, owner);
        }

        private async void RunSettingsButtonAnimationToShowAsync(Button button, IAnimatable owner)
        {
            await Task.Run(() => {
                new Animation((d) => button.WidthRequest = d, button.Width, _buttonWidthMax)
                    .Commit(owner, "ButtonShow", Constants.AnimationRate, Constants.AnimationLength, Easing.CubicInOut);
            });
        }

        private async void RunSettingsViewAnimationToShowAsync(View view, IAnimatable owner)
        {
            await Task.Run(() => {
                new Animation((d) => view.HeightRequest = d, view.Height, _settingsViewHeightMax)
                    .Commit(owner, "Show", Constants.AnimationRate, Constants.AnimationLength, Easing.SinInOut);
            });
        }

        private async void RunSettingsButtonAnimationToHideAsync(Button button, IAnimatable owner)
        {
            var buttonWidthMin = button.Text.Length * Constants.SymbolLength;
            await Task.Run(() => {
                new Animation((d) => button.WidthRequest = d, button.Width, buttonWidthMin)
                    .Commit(owner, "ButtonHide", Constants.AnimationRate, Constants.AnimationLength, Easing.CubicInOut);
            });
        }
        private async void RunSettingsViewAnimationToHideAsync(View view, IAnimatable owner)
        {
            await Task.Run(() => {
                new Animation((d) => view.HeightRequest = d, view.Height, _settingsViewHeightMin)
                    .Commit(owner, "Hide", Constants.AnimationRate, Constants.AnimationLength, Easing.SinInOut);
            });
        }

        private void ChangeButtonStyle(Button button, bool show = true)
        {
            if (show) ButtonStyleShow(button);
            else ButtonStyleHide(button);
        }

        private void ButtonStyleShow(Button button)
        {
            button.BackgroundColor = Constants.ColorMaterialBlue;
            button.TextColor = Color.White;
        }

        private void ButtonStyleHide(Button button)
        {
            button.BackgroundColor = Color.White;
            button.TextColor = Constants.ColorMaterialBlue;
        }
    }
}