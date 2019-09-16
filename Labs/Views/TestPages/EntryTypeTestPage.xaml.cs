using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Labs.ViewModels.Creators;
using Labs.ViewModels.Tests;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Labs.Views.TestPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntryTypeTestPage : ContentPage
    {
        public EntryTypeTestPage(string path, string fileName)
        {
            InitializeComponent();

            var entryViewModel = new EntryTypeTestViewModel(path, fileName);

            BindingContext = entryViewModel.GetSettingsModel;
        }

    }
}