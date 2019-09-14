using System.Collections.Generic;
using System.Threading.Tasks;
using Labs.Helpers;
using Labs.Models;
using Labs.Resources;

namespace Labs.ViewModels
{
    public class PageSettingsViewModel
    {
        public readonly PageSettingsModel SettingsModel;
        public PageSettingsViewModel()
        {
            SettingsModel = new PageSettingsModel();
        }

        private string CheckTimeAndPrice()
        {
            var message = string.Empty;
            SettingsModel.Price = CheckText(SettingsModel.Price);
            SettingsModel.Seconds = CheckText(SettingsModel.Seconds);
            message += string.IsNullOrEmpty(SettingsModel.Price) ? AppResources.WarningPrice + " \n" : "";
            message += string.IsNullOrEmpty(SettingsModel.Seconds) ? "Add seconds" + " \n" : "";

            return message;
        }
        public string CheckPageSettings()
        {
            var message = string.Empty;
            SettingsModel.Question = CheckText(SettingsModel.Question);
            message += string.IsNullOrEmpty(SettingsModel.Question) ? AppResources.WarningQuestion + " \n" : "";
            message += CheckTimeAndPrice();

            return message;
        }

        public string CheckCreatorMenuPageSettings()
        {
            var message = string.Empty;
            SettingsModel.Name = CheckText(SettingsModel.Name);
            SettingsModel.Subject = CheckText(SettingsModel.Subject);
            message += string.IsNullOrEmpty(SettingsModel.Name) ? "name" + " \n" : "";
            message += string.IsNullOrEmpty(SettingsModel.Subject) ? "subject" + " \n" : "";
            message += CheckTimeAndPrice();

            return message;
        }

        public async Task<IEnumerable<string>> GetPageSettingsAsync(bool all = false)
        {
            if (all) return GetSettingsForCreating();
            return await Task.Run(GetSettings);
        }

        private IEnumerable<string> GetSettings()
        {
            var settings = new List<string> {
                TimeHelper.NormalizeTime(SettingsModel.TimeSpan, SettingsModel.Seconds),
                SettingsModel.Price,
                SettingsModel.Question
            };

            return settings;
        }

        private IEnumerable<string> GetSettingsForCreating()
        {
            var settings = new List<string> {
                SettingsModel.Name,
                SettingsModel.Subject,
                TimeHelper.NormalizeTime(SettingsModel.TimeSpan, SettingsModel.Seconds),
                SettingsModel.Price
            };

            return settings;
        }

        private string CheckText(string text)
        {
            return string.IsNullOrEmpty(text) ? null : text.Trim();
        }

        //string time, string price, string question, string name = "", string subject = ""
        public void SetPageSettingsModel(params string[] settings)
        {
            TimeHelper.GetTime(settings[0], out var timeSpan, out var seconds);
            SettingsModel.TimeSpan = timeSpan;
            SettingsModel.Seconds = seconds;
            SettingsModel.Price = settings[1];
            SettingsModel.Question = settings[2];
        }

        public void SetMenuPageSettings(params string[] settings)
        {
            SettingsModel.Name = settings[0];
            SettingsModel.Subject = settings[1];
            TimeHelper.GetTime(settings[2], out var timeSpan, out var seconds);
            SettingsModel.TimeSpan = timeSpan;
            SettingsModel.Seconds = seconds;
            SettingsModel.Price = settings[3];
        }

        public void SetStartPageSettings(params string[] settings)
        {
            SettingsModel.Name = settings[0];
            SettingsModel.Subject = settings[1];
            SettingsModel.Time = settings[2];
            SettingsModel.Price = settings[3];
        }

        public static string FixText(string text, bool isSeconds = false)
        {
            if (text == string.Empty) return string.Empty;
            if (isSeconds && int.Parse(text) > 60) return "60";
            if (text.Length > 2) text = text.Remove(2);

            for (var i = 0; i < text.Length; i++) {
                if (text[i] == ',' || text[i] == '-') text = text.Remove(i, 1);
            }

            return text;
        }
    }
}
