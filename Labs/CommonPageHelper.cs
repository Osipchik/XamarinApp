using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace Labs
{
    static class CommonPageHelper
    {
        public static string GetFileName(string savingType, string path, string fileName)
        {
            string Name;
            if (fileName != null) Name = Path.Combine(path, fileName);
            else
            {
                int count = 0;
                while (true)
                {
                    Name = Path.Combine(path, savingType + "_Type_" + $"{count}" + ".txt");
                    if (File.Exists(Name) == false) break;
                    count++;
                }
            }

            return Name;
        }

        public static bool CheckCoast(string coast)
        {
            var coastText = coast;
            int result;
            if (int.TryParse(coastText, out result) == false) return false;
            if (result < 0 || result > 100) return false;

            return true;
        }

        public static string GetTypeName(string directoryName)
        {
            string name = "";
            foreach (var symbol in directoryName)
            {
                if (symbol == '_') break;
                name += symbol;
            }

            return name;
        }

        public static CheckBox GetCheckBox(bool isChecked)
        {
            return new CheckBox
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                IsChecked = isChecked
            };
        }

        public static Editor GetEditor(string text = "", string placeHolder = "Text")
        {
            return new Editor
            {
                Placeholder = placeHolder,
                VerticalOptions = LayoutOptions.Center,
                AutoSize = EditorAutoSizeOption.TextChanges,
                Text = text
            };
        }

        public static ImageButton GetDeleteButton()
        {
            return new ImageButton
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                BackgroundColor = Color.Transparent,
                Source = "Clear_Button.png",
                Aspect = Aspect.AspectFit,
                WidthRequest = 20,
                HeightRequest = 20
            };
        }

        public static Label GetLabel(string text = "")
        {
            return new Label
            {
                Text = text,
            };
        }
    }
}
