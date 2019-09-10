﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Labs.Helpers;
using Labs.Models;
using Labs.Resources;
using Labs.Views;
using Xamarin.Forms;

namespace Labs.ViewModels
{
    public class CreatorViewModel
    {
        public CreatorViewModel(string path)
        {
            GetPath = path;
        }
        public string GetPath { get; }

        public SettingsModel ReadSettings()
        {
            var settings = new SettingsModel();
            using (var reader = new StreamReader(Path.Combine(GetPath, Constants.SettingsFileTxt))) {
                settings.TestName = reader.ReadLine();
                settings.TestSubject = reader.ReadLine();
                PageHelper.GetTime(reader.ReadLine(), out var timeSpan, out var seconds);
                settings.SettingSpan = timeSpan;
                settings.Seconds = seconds;
            }

            return settings;
        }

        public async Task<bool> SaveTestAsync(SettingsModel settingsModel)
        {
            var result = false;
            var settings = await Task.Run(() => GetSettingsToSave(settingsModel));
            if (settings.Count != 0) {
                await Task.Run(() => Save(GetPath, settings));
                result = true;
            }

            return result;
        }

        public Dictionary<string, string> GetSettingsDictionary()
        {
            var settingsModel = ReadSettings();
            var infos = new Dictionary<string, string> {
                {"time", AppResources.TestSettingsTime + ' ' + PageHelper.NormalizeTime(settingsModel.SettingSpan, settingsModel.Seconds)},
                {"name", AppResources.TestSettingsName + ' ' + settingsModel.TestName},
                {"subject", AppResources.Subject + ' ' + settingsModel.TestSubject},
                {"price", AppResources.Price + ' ' + GetTotalCoast()},
            };

            return infos;
        }

        public async void DeleteFolderAsync(CreatorPage senderPage)
        {
            await Task.Run(() => { Directory.Delete(GetPath, true); });
            await Task.Run(() => MessagingCenter.Send<Page>(senderPage, Constants.HomeListUpload));
        }

        public async void CreateTempFolderAsync()
        {
            if (!Directory.Exists(GetPath)) await Task.Run(() => Directory.CreateDirectory(GetPath));
        }

        private List<string> GetSettingsToSave(SettingsModel settingsModel)
        {
            var settings = new List<string>();
            if (!string.IsNullOrEmpty(settingsModel.TestName) && !string.IsNullOrEmpty(settingsModel.TestSubject)) {
                settings.Add(settingsModel.TestName);
                settings.Add(settingsModel.TestSubject);
                settings.Add(PageHelper.NormalizeTime(settingsModel.SettingSpan, settingsModel.Seconds));
            }

            return settings;
        }

        private string GetTestPath(string path)
        {
            string testPath;
            int count = 0;
            do {
                testPath = Path.Combine(path, Constants.TestFolder, $"{Constants.TestName}{count}");
                count++;
            } while (Directory.Exists(testPath));

            return testPath;
        }

        private void Save(string directory, ICollection<string> settings)
        {
            File.WriteAllLines(Path.Combine(directory, Constants.SettingsFileTxt), settings, Encoding.UTF8);
            if (directory.Contains(Constants.TempFolder)) {
                var path = GetTestPath(directory.Remove(directory.LastIndexOf('/') + 1));
                MoveFiles(path, directory);
            }
        }

        private void MoveFiles(string destPath, string sourcePath)
        {
            Directory.CreateDirectory(destPath);
            foreach (var file in new DirectoryInfo(sourcePath).GetFiles()) {
                File.Move(Path.Combine(sourcePath, file.Name), Path.Combine(destPath, file.Name));
            }
        }

        private string GetTotalCoast()
        {
            var totalCoast = 0;
            foreach (var coast in new InfoViewModel(GetPath).GetFilesInfo()) {
                totalCoast += int.Parse(coast.Detail);
            }

            return totalCoast.ToString();
        }
    }
}