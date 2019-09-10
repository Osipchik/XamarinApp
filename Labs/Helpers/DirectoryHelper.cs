using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Labs.Helpers
{
    public class DirectoryHelper
    {
        public static string GetTypeName(string directoryName)
        {
            return directoryName.TakeWhile(symbol => symbol != '_').Aggregate("", (current, symbol) => current + symbol);
        }

        private static string GetFileName(string savingType, string path, string fileName)
        {
            return !string.IsNullOrEmpty(fileName) ? Path.Combine(path, fileName) : GenerateFileName(path, savingType);
        }
        private static string GenerateFileName(string path, string savingType)
        {
            var fileName = string.Empty;
            for (int i = 0; !File.Exists(fileName) && string.IsNullOrEmpty(fileName); i++) {
                fileName = Path.Combine(path, savingType + "_Type_" + $"{i}" + ".txt");
            }

            return fileName;
        }
        public static async Task<string> GetFileNameAsync(string savingType, string path, string fileName = "")
        {
            return await Task.Run(() => GetFileName(savingType, path, fileName));
        }

        public static async void SaveLinesToFile(string savingType, string path, string fileName, IEnumerable<string> stringsToSave)
        {
            File.WriteAllLines(await GetFileNameAsync(savingType, path, fileName), stringsToSave);
        }

        public static string[] ReadFile(string path, string fileName)
        {
            return File.ReadAllLines(GetFileName("", path, fileName));
        }
    }
}
