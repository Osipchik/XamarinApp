using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Labs.Annotations;
using Labs.Helpers;
using Labs.Models;

namespace Labs.ViewModels
{
    public class InfoViewModel : INotifyPropertyChanged
    {
        private List<InfoModel> _infoModels = new List<InfoModel>();
        public List<InfoModel> InfoModels
        {
            get => _infoModels;
            set
            {
                _infoModels = value;
                OnPropertyChanged();
            }
        }

        private readonly string _path;
        private readonly PropertyInfo[] _modelProperties;
        public InfoViewModel(string path)
        {
            _path = path;
            _modelProperties = typeof(InfoModel).GetProperties();
        }

        public string GetNameOfPropertyInModel(int index)
        {
            return _modelProperties[index].Name;
        }

        public string GetElementPath(int index)
        {
            if (InfoModels.Count == 0) {
                throw new ArgumentNullException();
            }

            return Path.Combine(_path, InfoModels[index].Name);
        }

        // TODO: make this async
        public async void GetFilesModelAsync()
        {
            await Task.Run(() =>
            {
                InfoModels.Clear();
                foreach (var info in new DirectoryInfo(_path).GetFiles()) {
                    if (info.Name == Constants.SettingsFileTxt) continue;
                    InfoModels.Add(GetFileModel(info));
                }
            });
        }

        // TODO: add async
        public List<InfoModel> GetFilesInfo()
        {
            InfoModels.Clear();
            foreach (var info in new DirectoryInfo(_path).GetFiles()) {
                if (info.Name == Constants.SettingsFileTxt) continue;
                InfoModels.Add(GetFileModel(info));
            }

            return InfoModels;
        }

        public async void SetDirectoriesInfoAsync()
        {
            await Task.Run(() =>
            {
                InfoModels.Clear();
                foreach (var dirInfo in new DirectoryInfo(_path).GetDirectories()) {
                    InfoModels.Add(GetDirectoryModel(dirInfo));
                }
            });
        }
        public List<InfoModel> GetDirectoryInfo()
        {
            InfoModels.Clear();
            foreach (var dirInfo in new DirectoryInfo(_path).GetDirectories()) {
                InfoModels.Add(GetDirectoryModel(dirInfo));
            }

            return InfoModels; 
        }

        private InfoModel GetDirectoryModel(DirectoryInfo dirInfo)
        {
            ReadSettings(out string title, out string detail, Path.Combine(dirInfo.FullName, Constants.SettingsFileTxt));
            return new InfoModel {
                Name = dirInfo.Name,
                Title = title,
                Detail = detail,
                Date = dirInfo.CreationTime.ToShortDateString()
            };
        }






        private InfoModel GetFileModel(FileInfo fileInfo)
        {
            var model = new InfoModel { Name = fileInfo.Name, Date = fileInfo.CreationTime.ToShortDateString() };
            ReadDetailAndTitleFromFile(out var price, out var question, fileInfo.FullName);
            model.Detail = price;
            model.Title = question;

            return model;
        }
        private void ReadDetailAndTitleFromFile(out string price, out string question, string filePath)
        {
            using (var reader = new StreamReader(filePath)) {
                reader.ReadLine();
                price = reader.ReadLine();
                question = reader.ReadLine();
            }

            question = CutText(question);
        }
        private string CutText(string text)
        {
            if (text.Length >= 30) {
                text = text.Remove(27);
                text += "...";
            }

            return text;
        }

        // TODO: add async
        private void ReadSettings(out string title, out string detail, string path)
        {
            using (var reader = new StreamReader(path)) {
                title = reader.ReadLine();
                detail = reader.ReadLine();
            }
        }






        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
