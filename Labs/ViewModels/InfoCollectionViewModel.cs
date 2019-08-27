using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Labs.Annotations;
using Labs.Models;

namespace Labs.ViewModels
{
    public sealed class InfoCollectionViewModel : IEnumerable<string>, INotifyPropertyChanged
    {
        public InfoCollectionViewModel()
        {
            InfosCollection = new ObservableCollection<InfoCollection>();
        }

        private ObservableCollection<InfoCollection> _infosCollection;
        public ObservableCollection<InfoCollection> InfosCollection
        {
            get => _infosCollection;
            set
            {
                _infosCollection = value;
                OnPropertyChanged();
            }
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
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    internal class DirectoryInfoListEnumerator : IEnumerator<string>
    {
        private readonly ObservableCollection<InfoCollection> _items;
        private int _position = -1;

        public DirectoryInfoListEnumerator(ObservableCollection<InfoCollection> list)
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
