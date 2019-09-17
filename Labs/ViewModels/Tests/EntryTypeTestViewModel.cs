using System.Threading.Tasks;
using Labs.Helpers;
using Labs.Models;

namespace Labs.ViewModels.Tests
{
    public class EntryTypeTestViewModel
    {
        private readonly SettingsViewModel _settingsViewModel;
        private string _rightAnswer;
        public  TimerViewModel TimerViewModel;
        public string Answer { get; set; }

        public EntryTypeTestViewModel(string path, string fileName, TimerViewModel testTimeViewModel)
        {
            _settingsViewModel = new SettingsViewModel();
            Initialize(path, fileName, testTimeViewModel);
        }

        private async void Initialize(string path, string fileName, TimerViewModel testTimeViewModel)
        {
            var strings = DirectoryHelper.ReadStringsFromFile(path, fileName);
            TimerViewModel = testTimeViewModel ?? new TimerViewModel(strings[0]);
            await Task.Run(() => {
                _settingsViewModel.SetPageSettingsModel(strings[0], strings[1], strings[2]);
                _rightAnswer = strings[3];
            });
        }

        public SettingsModel GetSettingsModel => _settingsViewModel.SettingsModel;
    }
}
