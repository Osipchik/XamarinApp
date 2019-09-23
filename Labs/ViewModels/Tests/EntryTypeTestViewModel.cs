using System.Threading.Tasks;
using Labs.Helpers;
using Labs.Models;
using Xamarin.Forms;

namespace Labs.ViewModels.Tests
{
    public class EntryTypeTestViewModel
    {
        private readonly SettingsViewModel _settingsViewModel;
        public TimerViewModel TimerViewModel;
        public EntryTestModel EntryModel { get; }

        public EntryTypeTestViewModel(string path, string fileName, TimerViewModel testTimeViewModel)
        {
            _settingsViewModel = new SettingsViewModel();
            EntryModel = new EntryTestModel();
            Initialize(path, fileName, testTimeViewModel);
        }

        private async void Initialize(string path, string fileName, TimerViewModel testTimeViewModel)
        {
            var strings = DirectoryHelper.ReadStringsFromFile(path, fileName);
            TimerViewModel = testTimeViewModel ?? new TimerViewModel(strings[0]);
            await Task.Run(() => {
                _settingsViewModel.SetPageSettingsModel(strings[0], strings[1], strings[2]);
                EntryModel.RightAnswer = strings[3];
                EntryModel.BorderColor = (Color)Application.Current.Resources["ColorMaterialBlue"];
            });
        }

        public SettingsModel GetSettingsModel => _settingsViewModel.SettingsModel;
        public TimerModel GeTimerModel => TimerViewModel.TimerModel;

        public async void CheckPageAsync(TestModel testModel)
        {
            EntryModel.IsReadOnly = true;
            await Task.Run(() => {
                if (EntryModel.Answer == EntryModel.RightAnswer) {
                    EntryModel.BorderColor = GetColor(true);
                    if (testModel == null) return;
                    testModel.Price += int.Parse(GetSettingsModel.Price);
                    testModel.RightAnswers++;
                }
                else EntryModel.BorderColor = GetColor(false);
            });

            await Task.Run(DisableTimer);
        }
        private void DisableTimer()
        {
            if (TimerViewModel != null) {
                TimerViewModel.TimerModel.TimerIsVisible = false;
                TimerViewModel.Index = null;
                TimerViewModel = null;
            }
        }

        private Color GetColor(bool isRight) =>
            isRight ? (Color)Application.Current.Resources["ColorMaterialGreen"] 
                    : (Color)Application.Current.Resources["ColorMaterialRed"];
    }
}
