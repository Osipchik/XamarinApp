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

        public string CheckQuestionPageSettings()
        {
            var message = string.Empty;
            message += string.IsNullOrEmpty(SettingsModel.Question) ? AppResources.WarningQuestion + " \n" : "";
            message += string.IsNullOrEmpty(SettingsModel.Price) ? AppResources.WarningPrice + " \n" : "";
            message += string.IsNullOrEmpty(SettingsModel.Seconds) ? "Add seconds" + " \n" : "";

            return message;
        }

        public async Task<IEnumerable<string>> GetPageSettingsAsync()
        {
            return await Task.Run(() => new List<string> {
                PageHelper.NormalizeTime(SettingsModel.TimeSpan, CheckText(SettingsModel.Seconds)),
                CheckText(SettingsModel.Price),
                CheckText(SettingsModel.Question)
            });
        }
        private string CheckText(string text)
        {
            return string.IsNullOrEmpty(text) ? "" : text;
        }

        public void SetPageSettingsModel(string time, string price, string question)
        {
            PageHelper.GetTime(time, out var timeSpan, out var seconds);
            SettingsModel.TimeSpan = timeSpan;
            SettingsModel.Seconds = seconds;
            SettingsModel.Price = price;
            SettingsModel.Question = question;
        }


    }
}
