using System.Threading.Tasks;
using Labs.Helpers;
using Xamarin.Forms;

namespace Labs.ViewModels
{
    public class CreatorAnimationViewModel
    {
        private readonly uint _buttonWidthMax, _settingsViewHeightMax, _settingsViewHeightMin;

        public CreatorAnimationViewModel(uint buttonWidthMax, uint settingsViewHeightMax, uint settingsViewHeightMin)
        {
            _buttonWidthMax = buttonWidthMax;
            _settingsViewHeightMax = settingsViewHeightMax;
            _settingsViewHeightMin = settingsViewHeightMin;
        }

        public void RunShowOrHideAnimation(Button button, TableView tableView, IAnimatable owner, bool show)
        {
            if (show) RunShowAnimation(button, tableView, owner);
            else RunHideAnimation(button, tableView, owner);
        }

        private void RunShowAnimation(Button button, TableView tableView, IAnimatable owner)
        {
            ChangeButtonStyle(button);
            RunSettingsButtonAnimationToShowAsync(button, owner);
            RunSettingsViewAnimationToShowAsync(tableView, owner);
        }
        private void RunHideAnimation(Button button, TableView tableView, IAnimatable owner)
        {
            ChangeButtonStyle(button, false);
            RunSettingsButtonAnimationToHideAsync(button, owner);
            RunSettingsViewAnimationToHideAsync(tableView, owner);
        }

        private async void RunSettingsButtonAnimationToShowAsync(Button button, IAnimatable owner)
        {
            await Task.Run(() => {
                new Animation((d) => button.WidthRequest = d, button.Width, _buttonWidthMax)
                    .Commit(owner, "ButtonShow", Constants.AnimationRate, Constants.AnimationLength, Easing.CubicInOut);
            });
        }
        private async void RunSettingsViewAnimationToShowAsync(TableView settingsTableView, IAnimatable owner)
        {
            await Task.Run(() => {
                new Animation((d) => settingsTableView.HeightRequest = d, settingsTableView.Height, _settingsViewHeightMax)
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
        private async void RunSettingsViewAnimationToHideAsync(TableView settingsTableView, IAnimatable owner)
        {
            await Task.Run(() => {
                new Animation((d) => settingsTableView.HeightRequest = d, settingsTableView.Height, _settingsViewHeightMin)
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
            button.BackgroundColor = Color.FromHex(Constants.ColorMaterialBlue);
            button.TextColor = Color.White;
        }

        private void ButtonStyleHide(Button button)
        {
            button.BackgroundColor = Color.White;
            button.TextColor = Color.FromHex(Constants.ColorMaterialBlue);
        }
    }
}