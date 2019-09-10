﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using Labs.Annotations;
using Labs.Helpers;
using Labs.Models;

using System.Windows.Input;

namespace Labs.ViewModels
{
    public class InfoViewModel : IEnumerable<string>, INotifyPropertyChanged
    {
        public IEnumerator<string> GetEnumerator()
        {
            return new DirectoryInfoListEnumerator(InfosModel);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private readonly string _path;
        private readonly PropertyInfo[] _modelProperties;
        public InfoViewModel(string path)
        {
            InfosModel = new ObservableCollection<InfoModel>();
            _path = path;
            _modelProperties = typeof(InfoModel).GetProperties();
        }

        public string GetNameOfPropertyInModel(int index)
        {
            return _modelProperties[index].Name;
        }

        public string GetElementPath(int index)
        {
            if (InfosModel.Count == 0) {
                throw new ArgumentNullException();
            }

            return Path.Combine(_path, InfosModel[index].Name);
        }

        private ObservableCollection<InfoModel> _infosModel;
        public ObservableCollection<InfoModel> InfosModel
        {
            get => _infosModel;
            set {
                _infosModel = value;
                OnPropertyChanged();
            }
        }

        // TODO: add async
        public ObservableCollection<InfoModel> GetFilesInfo()
        {
            InfosModel.Clear();
            foreach (var info in new DirectoryInfo(_path).GetFiles()) {
                if (info.Name == Constants.SettingsFileTxt) continue;
                InfosModel.Add(GetFileModel(info));
            }

            return InfosModel;
        }

        // TODO: add async
        public ObservableCollection<InfoModel> GetDirectoryInfo()
        {
            InfosModel.Clear();
            foreach (var dirInfo in new DirectoryInfo(_path).GetDirectories()) {
                InfosModel.Add(GetDirectoryModel(dirInfo));
            }

            return InfosModel; 
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
            var collection = new InfoModel { Name = fileInfo.Name, Date = fileInfo.CreationTime.ToShortDateString() };
            ReadDetailAndTitleFromFile(out var detail, out var title, fileInfo.FullName);
            collection.Detail = detail;
            collection.Title = title;

            return collection;
        }
        private void ReadDetailAndTitleFromFile(out string detail, out string title, string filePath)
        {
            using (var reader = new StreamReader(filePath))
            {
                reader.ReadLine();
                detail = reader.ReadLine();
                title = reader.ReadLine();
            }

            title = CutText(title);
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
    }

    internal class DirectoryInfoListEnumerator : IEnumerator<string>
    {
        private readonly ObservableCollection<InfoModel> _items;
        private int _position = -1;

        public DirectoryInfoListEnumerator(ObservableCollection<InfoModel> list) => _items = list;
        public string Current
        {
            get {
                if (_position == -1 || _position >= _items.Count) {
                    throw new InvalidOperationException();
                }
                return _items[_position].Name;
            }
        }

        object IEnumerator.Current => throw new NotImplementedException();

        public bool MoveNext()
        {
            if (_position < _items.Count - 1) {
                _position++;
                return true;
            }
            else return false;
        }

        public void Reset() => _position = -1;
        public void Dispose() { }
    }
}
