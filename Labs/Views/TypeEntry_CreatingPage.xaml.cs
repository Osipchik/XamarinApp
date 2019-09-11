using System;
using System.IO;
using System.Threading.Tasks;
using Labs.Helpers;
using Labs.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Labs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TypeEntryCreatingPage
    {
        private readonly string _path;
        private readonly string _fileName;

        public TypeEntryCreatingPage(string path)
        {
            InitializeComponent();
            _path = path;
        }

        public TypeEntryCreatingPage(string path, string fileName) : this(path)
        {
            _fileName = fileName;
            
            AddDeleteToolBarItem();
        }

        private void AddDeleteToolBarItem()
        {
            var item = new ToolbarItem
            {
                Text = AppResources.Delete,
                Order = ToolbarItemOrder.Secondary
            };
            //item.Clicked += ItemDelete_OnClicked;
            ToolbarItems.Add(item);
        }

        private void ChooseItemsToDelete_OnClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ItemDeleteFileAsync_OnClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void SaveButton_OnClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void _entrySeconds_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Coast_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void HideOrShowAsync_OnClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}