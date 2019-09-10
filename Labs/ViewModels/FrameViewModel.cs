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
        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


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
                    BorderColor = Constants.ColorMaterialGray,
                    ItemTextLeft = string.Empty,
                    EditorLeftIsReadOnly = true,
                    isRight = false
                });
            });
        }

        public async void AddModelAsync(string textLeft, bool _isRight)
        {
            await Task.Run(() => {
                Models.Add(new FrameModel
                {
                    BorderColor = GetColor(_isRight),
                    ItemTextLeft = textLeft,
                    EditorLeftIsReadOnly = true,
                    isRight = _isRight,
                });
            });
        }

        public async void DisableAllAsync()
        {
            DisableLastItem();
            await Task.Run(() => {
                foreach (var model in Models) {
                    model.BorderColor = model.isRight ? Constants.ColorMaterialGreen : Constants.ColorMaterialGray;
                }
            });
        }

        public void ItemIsWriteAble(int index)
        {
            DisableLastItem();
            _itemIndex = index;
            Models[_itemIndex].EditorLeftIsReadOnly = false;
            Models[_itemIndex].BorderColor = Constants.ColorMaterialBlue;
        }

        public void DisableLastItem()
        {
            if (_itemIndex >= 0)
            {
                Models[_itemIndex].EditorLeftIsReadOnly = true;
                Models[_itemIndex].BorderColor = Models[_itemIndex].isRight
                    ? Constants.ColorMaterialGreen
                    : Constants.ColorMaterialGray;
            }
            _itemIndex = -1;
        }

        public void RightItems(int index)
        {
            Models[index].isRight = !Models[index].isRight;
            Models[index].BorderColor = GetColor(Models[index].isRight);
        }

        private Color GetColor(bool isRight)
        {
            return isRight ? Constants.ColorMaterialGreen : Constants.ColorMaterialGray;
        }

        public void ItemToDelete(int index)
        {
            if (Models[index].BorderColor == Constants.ColorMaterialRed) {
                _itemIndexToDeleteList.Remove(index);
                Models[index].BorderColor = Models[index].isRight ? Constants.ColorMaterialGreen : Constants.ColorMaterialGray;
            }
            else {
                _itemIndexToDeleteList.Add(index);
                Models[index].BorderColor = Constants.ColorMaterialRed;
            }
        }

        public async void DeleteItemsAsync()
        {
            await Task.Run(() =>
            {
                foreach (var model in GetItemsToDelete()) {
                    Models.Remove(model);
                }
            });
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

        public void SetText(string text)
        {
            if (_itemIndex >= 0) {
                Models[_itemIndex].ItemTextLeft = text;
            }
        }
    }
}
