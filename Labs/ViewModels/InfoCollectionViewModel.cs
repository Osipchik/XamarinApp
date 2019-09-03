using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using Labs.Annotations;
using Labs.Helpers;
using Labs.Models;

namespace Labs.ViewModels
{
    public sealed class InfoCollectionViewModel : IEnumerable<string>, INotifyPropertyChanged
    {
        public InfoCollectionViewModel()
        {
            InfosCollection = new ObservableCollection<InfoModel>();
            
        }

        private ObservableCollection<InfoModel> _infosCollection;
        public ObservableCollection<InfoModel> InfosCollection
        {
            get => _infosCollection;
            set {
                _infosCollection = value;
                OnPropertyChanged();
            }
        }

        // TODO: add async
        public ObservableCollection<InfoModel> GetFilesInfo(string path)
        {
            InfosCollection.Clear();
            foreach (var info in new DirectoryInfo(path).GetFiles()) {
                if (info.Name == Constants.SettingsFileTxt) continue;
                InfosCollection.Add(GetFileInfo(info));
            }

            return InfosCollection;
        }

        private InfoModel GetFileInfo(FileInfo fileInfo)
        {
            var collection = new InfoModel{ Name = fileInfo.Name, Date = fileInfo.CreationTime.ToShortDateString() };
            using (var reader = new StreamReader(fileInfo.FullName)) {
                collection.Detail = reader.ReadLine();
                reader.ReadLine();
                collection.Title = reader.ReadLine();
            }

            return collection;
        }

        // TODO: fix this
        public ObservableCollection<InfoModel> GetDirectoryInfo(DirectoryInfo directoryInfo)
        {
            InfosCollection.Clear();
            foreach (var dirInfo in directoryInfo.GetDirectories())
            {
                string title, detail;
                using (var reader = new StreamReader(Path.Combine(dirInfo.FullName, Constants.SettingsFileTxt)))
                {
                    title = reader.ReadLine();
                    detail = reader.ReadLine();
                }

                InfosCollection.Add(new InfoModel
                {
                    Name = dirInfo.Name,
                    Title = title,
                    Detail = detail,
                    Date = dirInfo.CreationTime.ToShortDateString()
                });
            }

            return InfosCollection; 
        }


        public IEnumerator<string> GetEnumerator()
        {
            return new DirectoryInfoListEnumerator(InfosCollection);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    internal class DirectoryInfoListEnumerator : IEnumerator<string>
    {
        private readonly ObservableCollection<InfoModel> _items;
        private int _position = -1;

        public DirectoryInfoListEnumerator(ObservableCollection<InfoModel> list)
        {
            _items = list;
        }

        public string Current
        {
            get
            {
                if (_position == -1 || _position >= _items.Count)
                    throw new InvalidOperationException();
                return _items[_position].Name;
            }
        }

        object IEnumerator.Current => throw new NotImplementedException();

        public bool MoveNext()
        {
            if (_position < _items.Count - 1)
            {
                _position++;
                return true;
            }
            else
                return false;
        }

        public void Reset()
        {
            _position = -1;
        }
        public void Dispose() { }
    }
}
