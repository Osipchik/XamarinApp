using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using Labs.Annotations;
using Labs.Helpers;
using Labs.Models;
using System.Threading.Tasks;

namespace Labs.ViewModels
{
    public class FrameViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<FrameModel> _models = new ObservableCollection<FrameModel>();
        public ObservableCollection<FrameModel> Models
        {
            get { return _models; }
            set
            {
                _models = value;
                OnPropertyChanged();
            }
        }

        private int _itemIndex = -1;
        private readonly List<int> _itemIndexToDeleteList;
        public FrameViewModel()
        {
            _itemIndexToDeleteList = new List<int>();
        }

        public async void AddNewModelAsync()
        {
            await Task.Run(() => {
                Models.Add(new FrameModel {
                    BorderColor = Constants.Colors.ColorMaterialGray,
                    ItemTextLeft = string.Empty,
                    ItemTextRight = string.Empty,
                    IsRight = false
                });
            });
        }

        public async void DisableAllAsync()
        {
            DisableLastItem();
            await Task.Run(() => {
                foreach (var model in Models) {
                    model.BorderColor = model.IsRight ? Constants.Colors.ColorMaterialGreen : Constants.Colors.ColorMaterialGray;
                }
            });
        }
        public void SelectItem(int index, bool disablePrevious = true)
        {
            if (disablePrevious) {
                DisableLastItem();
            }
            _itemIndex = index;
            var isTrue = Models[_itemIndex].BorderColor == Constants.Colors.ColorMaterialBlue;
            Models[_itemIndex].BorderColor = isTrue 
                ? Constants.Colors.ColorMaterialGray 
                : Constants.Colors.ColorMaterialBlue;
        }
        public void DisableLastItem()
        {
            if (_itemIndex >= 0)
            {
                Models[_itemIndex].BorderColor = Models[_itemIndex].IsRight
                    ? Constants.Colors.ColorMaterialGreen
                    : Constants.Colors.ColorMaterialGray;
            }
            _itemIndex = -1;
        }

        public void RightItems(int index)
        {
            Models[index].IsRight = !Models[index].IsRight;
            Models[index].BorderColor = GetColor(Models[index].IsRight);
        }

        public static Color GetColor(bool isRight) =>
            isRight ? Constants.Colors.ColorMaterialGreen : Constants.Colors.ColorMaterialGray;

        public static Color GetColorOnCheck(bool isRight) =>
            isRight ? Constants.Colors.ColorMaterialGreen : Constants.Colors.ColorMaterialRed;
        

        public void ItemToDelete(int index)
        {
            if (Models[index].BorderColor == Constants.Colors.ColorMaterialRed) {
                _itemIndexToDeleteList.Remove(index);
                Models[index].BorderColor = Models[index].IsRight 
                    ? Constants.Colors.ColorMaterialGreen 
                    : Constants.Colors.ColorMaterialGray;
            }
            else {
                _itemIndexToDeleteList.Add(index);
                Models[index].BorderColor = Constants.Colors.ColorMaterialRed;
            }
        }
        public void DeleteItems()
        {
            foreach (var model in GetItemsToDelete()) {
                Models.Remove(model);
            }
        }
        private IEnumerable<FrameModel> GetItemsToDelete()
        {
            var list = new List<FrameModel>();
            foreach (var i in _itemIndexToDeleteList) {
                list.Add(Models[i]);
            }
            _itemIndexToDeleteList.Clear();

            return list;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    internal static class ListExtensions
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            Random rnd = new Random();
            while (n > 1) {
                int k = (rnd.Next(0, n) % n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
