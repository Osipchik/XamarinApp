﻿using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Labs.Models;
using Labs.Resources;
using Xamarin.Forms;

namespace Labs.Helpers
{
    static class PageHelper
    {
        public static bool CheckCoast(string coast)
        {
            var coastText = coast;
            int result;
            if (int.TryParse(coastText, out result) == false) return false;
            if (result < 0 || result > 100) return false;

            return true;
        }

        public static Editor GetEditor(string text = "", string placeHolder = "Text")
        {
            return new Editor {
                Placeholder = placeHolder,
                VerticalOptions = LayoutOptions.Center,
                AutoSize = EditorAutoSizeOption.TextChanges,
                Text = text
            };
        }

        public static ImageButton GetDeleteButton()
        {
            return new ImageButton {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                BackgroundColor = Color.Transparent,
                Source = "Clear_Button.png",
                Aspect = Aspect.AspectFit,
                WidthRequest = 20,
                HeightRequest = 20
            };
        }






        public static async void CheckEntry(object sender, string text)
        {
            var entry = sender as Entry;
            entry.Text = await Task.Run(() => FixText(text));
        }

        private static string FixText(string text)
        {
            for (var i = 0; i < text.Length; i++){
                if (text[i] == ',') text = text.Remove(i, 1);
            }
            if (text.Length > 2) text = text.Remove(2);

            return text;
        }

        public static string CheckQuestionPageSettings(string question, string price, string seconds, int count)
        {
            var message = string.Empty;
            message += string.IsNullOrEmpty(question) ? AppResources.WarningQuestion + " \n" : "";
            message += string.IsNullOrEmpty(price) ? AppResources.WarningPrice + " \n" : "";
            message += string.IsNullOrEmpty(seconds) ? "Add seconds" + " \n" : "";
            message += count < 1 ? "Add frame" : "";

            return message;
        }

        public static string NormalizeTime(TimeSpan timeSpan, string seconds)
        {
            return timeSpan.ToString().Remove(6) + (seconds.Length < 2 ? "00" : seconds);
        }

        public static void GetTime(string time, out TimeSpan timeSpan, out string seconds)
        {
            var timeStrings = time.Split(':');
            timeSpan = new TimeSpan(int.Parse(timeStrings[0]), int.Parse(timeStrings[1]), 00);
            seconds = timeStrings[2];
        }
    }
}
