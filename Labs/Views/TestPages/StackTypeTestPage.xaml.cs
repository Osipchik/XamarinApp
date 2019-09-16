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
    public partial class StackTypeTestPage : ContentPage
    {
        private readonly StackTypeTestViewModel _stackViewModel;
        public StackTypeTestPage(string path, string fileName)
        {
            InitializeComponent();

            _stackViewModel = new StackTypeTestViewModel(path, fileName);
            SetBindings();
        }
        private void SetBindings()
        {
            BindingContext = _stackViewModel.GetSettingsModel;
            ListView.BindingContext = _stackViewModel.FrameViewModel;
        }
        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }
        private void ListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            _stackViewModel.TapEvent(e.ItemIndex);
        }
    }
}