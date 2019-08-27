using System.Collections.ObjectModel;

namespace Labs.Models
{
    public class Language
    {
        public string DisplayName { get; set; }
        public string ShortName { get; set; }

        public static ObservableCollection<Language> GetLanguageList()
        {
            var languages = new ObservableCollection<Language>()
            {
                new Language { DisplayName =  "Deutsche - German", ShortName = "de-DE" },
                new Language { DisplayName =  "Español - Spanish", ShortName = "es-ES" },
                new Language { DisplayName =  "Français - French", ShortName = "fr" },
                new Language { DisplayName =  "Italiano - italian", ShortName = "it"},
                new Language { DisplayName =  "日本語 - Japanese", ShortName = "ja" },
                new Language { DisplayName =  "한국어 - Korean", ShortName = "ko" },
                new Language { DisplayName =  "English", ShortName = "en" },
                new Language { DisplayName =  "Русский - Russian", ShortName = "ru-RU" },
                new Language { DisplayName =  "中文 - Chinese (simplified)", ShortName = "zh-Hans" },
                new Language { DisplayName =  "Chinese(Traditional)", ShortName = "zh-Hant" },
            };

            return languages;
        }
    }
}
