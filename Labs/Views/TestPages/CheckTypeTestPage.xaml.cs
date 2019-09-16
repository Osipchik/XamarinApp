using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Labs.ViewModels.Tests;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Labs.Views.TestPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CheckTypeTestPage : ContentPage
    {
        private readonly CheckTypeTestViewModel _checkViewModel;
        public CheckTypeTestPage(string path, string fileName)
        {
            InitializeComponent();

            _checkViewModel = new CheckTypeTestViewModel(path, fileName);
            SetBindings();
        }

        private void SetBindings()
        {
            BindingContext = _checkViewModel.GetSettingsModel;
            ListView.BindingContext = _checkViewModel.FrameViewModel;
        }

        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }
        private void ListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            _checkViewModel.TapEvent(e.ItemIndex);
        }
    }
}