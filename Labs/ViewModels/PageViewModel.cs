using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using Labs.Models;
using Labs.Views;
using Xamarin.Forms;

namespace Labs.ViewModels
{
    internal sealed class PageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // эти списки хронят ответы и индексы страниц
        private List<CheckTypePageView> _checkTypePages;
        private List<EntryTypePageView> _entryTypePages;
        private List<StackTypePageView> _stackTypePages;

        private bool[] _isDisable;

        public PageViewModel()
        {
            ItemsSource = new ObservableCollection<View>();

            _checkTypePages = new List<CheckTypePageView>();
            _entryTypePages = new List<EntryTypePageView>();
            _stackTypePages = new List<StackTypePageView>();
        }

        private ObservableCollection<View> _itemsSource;
        public ObservableCollection<View> ItemsSource
        {
            get { return _itemsSource; }
            set
            {
                _itemsSource = value;
                OnPropertyChanged("ItemsSource");
            }
        }

        public async void InitContentAsync(string path)
        {
            int index = 0;
            foreach (var info in InfoCollection.GetFilesInfo(new DirectoryInfo(path)))
            {
                switch (CommonPageHelper.GetTypeName(info.Name))
                {
                    case "Check":
                        ItemsSource.Add(new ScrollView
                        {
                            Content = await Task.Run(()=>
                                CheckTypePageView.GetCheckTypeLayout(Path.Combine(path, info.Name), index, ref _checkTypePages)),
                            Padding = 5
                        });
                        break;
                    case "Entry":
                        ItemsSource.Add(new ScrollView
                        {
                            Content = await Task.Run(() => 
                                    EntryTypePageView.GetEntryTypeLayout(Path.Combine(path, info.Name), index, ref _entryTypePages)),
                            Padding = 5
                        });
                        break;
                    case "Stack":
                        ItemsSource.Add(new ScrollView
                        {
                            Content = await Task.Run(() => 
                                    StackTypePageView.GetStackTypePage(Path.Combine(path, info.Name), index, ref _stackTypePages)),
                            Padding = 5
                        });
                        break;
                }

                index++;
            }

            _isDisable = new bool[index];
        }

        public void CheckIt(ref int coast, ref int rightCount)
        {
            foreach (var page in _checkTypePages)
            {
                page.CheckIt(ref _itemsSource, ref coast, ref rightCount);
            }

            foreach (var page in _entryTypePages)
            {
                page.CheckIt(ref _itemsSource, ref coast, ref rightCount);
            }

            foreach (var page in _stackTypePages)
            {
                page.CheckIt(ref _itemsSource, ref coast, ref rightCount);
            }
        }

        public async void DisableAsync(int index)
        {
            _isDisable[index] = true;

            await Task.Run(() =>
            {
                foreach (var page in _checkTypePages)
                {
                    if (index != page.Index) continue;
                    page.Disable(ref _itemsSource);
                    return;
                }
            });

            await Task.Run(() =>
            {
                foreach (var page in _entryTypePages)
                {
                    if (index != page.Index) continue;
                    page.Disable(ref _itemsSource);
                    return;
                }
            });

            await Task.Run(() =>
            {
                foreach (var page in _stackTypePages)
                {
                    if (index != page.Index) continue;
                    page.Disable(ref _itemsSource);
                    return;
                }
            });
        }

        public void DisableAll()
        {
            for(int i = 0, total = _itemsSource.Count; i < total; i++)
            {
                DisableAsync(i);
            }
        }

        public bool IsDisable(int index)
        {
            if(index >= _isDisable.Length) throw new ArgumentOutOfRangeException(nameof(index));
            return _isDisable[index];
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}