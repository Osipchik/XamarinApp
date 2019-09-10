using System;
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
    public sealed class InfoViewModel : IEnumerable<string>, INotifyPropertyChanged
    {
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
            using (var reader = new StreamReader(fileInfo.FullName)) {
                collection.Detail = reader.ReadLine();
                reader.ReadLine();
                collection.Title = reader.ReadLine();
            }

            return collection;
        }
        // TODO: add async
        private void ReadSettings(out string title, out string detail, string path)
        {
            using (var reader = new StreamReader(path)) {
                title = reader.ReadLine();
                detail = reader.ReadLine();
            }
        }

        public IEnumerator<string> GetEnumerator()
        {
            return new DirectoryInfoListEnumerator(InfosModel);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #region INotifyPropertyChanged 
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
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
