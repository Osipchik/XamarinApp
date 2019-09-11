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

        public TypeEntryCreatingPage(string path, string fileName = "")
        {
            InitializeComponent();
            _fileName = fileName;
            _path = path;
        }


        private void ItemDeleteFileAsync_OnClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void SaveButton_OnClicked(object sender, EventArgs e)
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