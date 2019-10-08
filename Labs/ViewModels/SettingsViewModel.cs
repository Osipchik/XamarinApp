using System;
using System.Security.Cryptography;
using Labs.Data;
using Labs.Models;
using Labs.Resources;
using Realms;

namespace Labs.ViewModels
{
    public class SettingsViewModel
    {
        public readonly SettingsModel SettingsModel;
        public SettingsViewModel()
        {
            SettingsModel = new SettingsModel();
        }

        public void SetEmptyModel()
        {
            SettingsModel.TimeSpan = TimeSpan.Zero;
            SettingsModel.Question = string.Empty;
            SettingsModel.Subject = string.Empty;
            SettingsModel.Name = string.Empty;
            SettingsModel.TotalPrice = "0";
            SettingsModel.Seconds = "0";
        }

        public void SetSettingsModel(string question, string price, string timeSpan)
        {
            SettingsModel.Question = question;
            SettingsModel.Price = price;
            SettingsModel.TimeSpan = TimeSpan.Parse(timeSpan);
            SettingsModel.Seconds = SettingsModel.TimeSpan.Seconds.ToString();
        }

        public SettingsModel GetSettingsToSave()
        {
            SettingsModel.TimeSpan += TimeSpan.Parse("00:00:" + SettingsModel.Seconds);
            return SettingsModel;
        }

        public void SetTestSettingsModel(Realm realm, TestModel testModel)
        {
            SettingsModel.Name = testModel.Name;
            SettingsModel.Subject = testModel.Subject;
            SettingsModel.TimeSpan = TimeSpan.Parse(testModel.Time);
            SettingsModel.TotalPrice = Repository.GetTotalPrice(realm, testModel);
        }

        //TODO проверить на число секунды и цену
        private void CheckPrice()
        {
            var message = CheckText(SettingsModel.Price) ? AppResources.WarningPrice + " \n" : "";
            foreach (var i in SettingsModel.Price)
            {
                //if()
            }

        }

        public string CheckPageSettings()
        {
            var message = string.Empty;
            message += CheckText(SettingsModel.Question) ? AppResources.WarningQuestion + " \n" : "";
            CheckPrice();

            return message;
        }

        public string CheckCreatorMenuPageSettings()
        {
            var message = string.Empty;
            {
                message += CheckText(SettingsModel.Name) ? AppResources.AddName + " \n" : "";
                message += CheckText(SettingsModel.Subject) ? AppResources.AddSubjectName + " \n" : "";
            }
            return message;
        }

        public bool CheckText(string text)
        {
            text = string.IsNullOrEmpty(text) ? null : text.Trim();
            return string.IsNullOrEmpty(text);
        }

        public static string FixText(string text, bool isSeconds = false)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            if (isSeconds && int.Parse(text) > 60) return "60";
            if (text.Length > 2) text = text.Remove(2);

            for (var i = 0; i < text.Length; i++) {
                if (text[i] == ',' || text[i] == '-') text = text.Remove(i, 1);
            }

            return text;
        }
    }
}
