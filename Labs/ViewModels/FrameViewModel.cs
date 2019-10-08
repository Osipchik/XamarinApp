using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Labs.Annotations;
using Labs.Models;
using System.Threading.Tasks;
using System.Windows.Input;
using Labs.Data;
using Xamarin.Forms;

namespace Labs.ViewModels
{
    public sealed class FrameViewModel : INotifyPropertyChanged
    {
        public enum Mode : int
        {
            ItemDelete = -1,
            ItemSelect,
            ItemRight
        }

        public ObservableCollection<FrameModel> Models { get; }
        private int _itemIndex = -1;
        private readonly List<int> _itemIndexToDeleteList;

        public FrameViewModel()
        {
            Models = new ObservableCollection<FrameModel>();
            _itemIndexToDeleteList = new List<int>();
            GetContentsToDelete = new List<QuestionContent>();
        }

        public ICommand AddItemCommand => new Command(AddEmptyModel);

        private Mode _modificator;
        public Mode Modificator
        {
            get => _modificator;
            set
            {
                _modificator = value;
                if (_modificator != 0){
                    DisableLastItem();
                }
            }
        }

        public async void TapEvent(int index)
        {
            switch (Modificator)
            {
                case Mode.ItemDelete:
                    await Task.Run(() => { ItemToDelete(index); });
                    break;
                case Mode.ItemRight:
                    await Task.Run(() => { RightItems(index); });
                    break;
                default:
                    await Task.Run(() => { SelectItem(index); });
                    break;
            }
        }

        public IList<QuestionContent> GetContentsToDelete { get; private set; }

        public void AddEmptyModel()
        {
            Models.Add(new FrameModel
            {
                Id = Guid.NewGuid().ToString(),
                BorderColor = (Color)Application.Current.Resources["ColorMaterialGray"],
                MainText = string.Empty,
                Text = string.Empty,
                IsRight = false
            });
        }

        public void FillCreatorFrames(IEnumerable<QuestionContent> contents, bool withColor = false)
        {
            foreach (var content in contents)
            {
                Models.Add(new FrameModel
                {
                    Id = content.Id,
                    Content = content,
                    MainText = content.MainText,
                    Text = content.Text,
                    IsRight = content.IsRight,
                    BorderColor = GetColor(withColor && content.IsRight)
                });
            }
        }

        public void FillTestFrames(IEnumerable<QuestionContent> contents, out IList<string> textList)
        {
            textList = new List<string>();
            foreach (var content in contents) {
                Models.Add(new FrameModel
                {
                    Id = content.Id,
                    Text = content.Text,
                    TextUnderMain = content.Text,
                    MainText = content.MainText,
                    IsRight = content.IsRight,
                    BorderColor = GetColor(false)
                });
                textList.Add(content.Text);
            }
        }

        public async void DisableAllAsync()
        {
            DisableLastItem();
            await Task.Run(() => {
                foreach (var model in Models) {
                    model.BorderColor = GetColor(model.IsRight);
                }
            });
        }

        public void SelectItem(int index, bool disablePrevious = true)
        {
            if (disablePrevious) {
                DisableLastItem();
            }
            _itemIndex = index;
            var isTrue = Models[_itemIndex].BorderColor == (Color) Application.Current.Resources["ColorMaterialBlue"];
            Models[_itemIndex].BorderColor = GetColor(isTrue, true);
        }

        private void DisableLastItem()
        {
            if (_itemIndex >= 0) {
                Models[_itemIndex].BorderColor = GetColor(Models[_itemIndex].IsRight);
            }
            _itemIndex = -1;
        }

        private void RightItems(int index)
        {
            Models[index].IsRight = !Models[index].IsRight;
            Models[index].BorderColor = GetColor(Models[index].IsRight);
        }

        private static Color GetColor(bool isRight, bool greenToBlue = false)
        {
            Color color = default;
            if (greenToBlue) {
                color = isRight ? (Color)Application.Current.Resources["ColorMaterialGray"]
                                : (Color)Application.Current.Resources["ColorMaterialBlue"];
            }
            else {
                color = isRight ? (Color)Application.Current.Resources["ColorMaterialGreen"]
                                : (Color)Application.Current.Resources["ColorMaterialGray"];
            }

            return color;
        }

        public static Color GetColorOnCheck(bool isRight) =>
            isRight ? (Color)Application.Current.Resources["ColorMaterialGreen"] 
                    : (Color)Application.Current.Resources["ColorMaterialRed"];

        private void ItemToDelete(int index)
        {
            if (Models[index].BorderColor == (Color)Application.Current.Resources["ColorMaterialRed"]) {
                _itemIndexToDeleteList.Remove(index);
                Models[index].BorderColor = Models[index].IsRight 
                    ? (Color)Application.Current.Resources["ColorMaterialGreen"]
                    : (Color)Application.Current.Resources["ColorMaterialGray"];
            }
            else {
                _itemIndexToDeleteList.Add(index);
                Models[index].BorderColor = (Color)Application.Current.Resources["ColorMaterialRed"];
            }
        }

        public void DeleteItems()
        {
            GetContentsToDelete.Clear();
            foreach (var i in _itemIndexToDeleteList) {
                GetContentsToDelete.Add(Models[i].Content);
                Models.Remove(Models[i]);
            }
            _itemIndexToDeleteList.Clear();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
