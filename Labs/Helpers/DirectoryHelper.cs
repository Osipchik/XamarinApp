using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Labs.Helpers
{
    public static class DirectoryHelper
    {
        public static string GetTypeName(string directoryName) =>
            directoryName.TakeWhile(symbol => symbol != '_').Aggregate("", (current, symbol) => current + symbol);
        
        private static string GetFileName(string savingType, string path, string fileName) =>
            !string.IsNullOrEmpty(fileName) ? Path.Combine(path, fileName) : GenerateFileName(path, savingType);
        
        private static string GenerateFileName(string path, string savingType)
        {
            var fileName = string.Empty;
            for (int i = 0; File.Exists(fileName) || string.IsNullOrEmpty(fileName); i++) {
                fileName = Path.Combine(path, savingType + "_Type_" + $"{i}" + ".txt");
            }

            return fileName;
        }

        public static void SaveFile(string savingType, string path, string fileName, IEnumerable<string> stringsToSave) =>
            File.WriteAllLines(GetFileName(savingType, path, fileName), stringsToSave, Encoding.UTF8);

        public static string[] ReadStringsFromFile(string path, string fileName)
        {
            var filePath = GetFileName("", path, fileName);
            return File.Exists(filePath) ? File.ReadAllLines(filePath) : null;
        }

        private static string GenerateTestPath(string path)
        {
            var testPath = string.Empty;
            for (int i = 0; Directory.Exists(testPath) || string.IsNullOrEmpty(testPath); i++) {
                testPath = Path.Combine(path, Constants.TestFolder, $"{Constants.TestName}{i}");
            }

            return testPath;
        }

        public static void SaveTest(string path, IEnumerable<string> settings)
        {
            File.WriteAllLines(Path.Combine(path, Constants.SettingsFileTxt), settings, Encoding.UTF8);
            if (path.Contains(Constants.TempFolder)) {
                MoveFiles(path);
            }
        }

        private static void MoveFiles(string sourcePath)
        {
            var destPath = GenerateTestPath(sourcePath.Remove(sourcePath.LastIndexOf('/') + 1));
            Directory.CreateDirectory(destPath);
            foreach (var file in new DirectoryInfo(sourcePath).GetFiles()) {
                File.Move(Path.Combine(sourcePath, file.Name), Path.Combine(destPath, file.Name));
            }
        }
    }
}
