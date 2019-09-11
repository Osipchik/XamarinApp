using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Labs.Helpers;
using Labs.Models;
using Labs.Resources;
using Xamarin.Forms;

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
