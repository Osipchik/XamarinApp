using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Labs.Helpers;
using Labs.Models;
using Labs.ViewModels;
using Lottie.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Labs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage
    {
        private readonly HomeViewModel _homeViewModel;
        private Frame _tappedFrame;
        [Obsolete]
        public HomePage()
        {
            InitializeComponent();

            _homeViewModel = new HomeViewModel(GridButtons, LabelName, LabelSubject, LabelDate);

            ListUploadAsync();
            Subscribe();

            LabelDateTapFunc();
            LabelNameTapFunc();
            LabelSubjectTapFunc();
        }

        private void ListUploadAsync() => listView.ItemsSource = _homeViewModel.GetModels.GetDirectoryInfo();
        
        private void Subscribe() =>
            MessagingCenter.Subscribe<Page>(this, Constants.HomeListUpload, (sender) =>
            {
                ListUploadAsync();
            });
        

        private async void ListView_OnItemTapped(object sender, ItemTappedEventArgs e) =>
            await Navigation.PushAsync(new StartTestPage(_homeViewModel.GetModels.GetElementPath(e.ItemIndex)));

        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e) =>
            ((ListView) sender).SelectedItem = null;
        
        private void SearchBar_OnTextChanged(object sender, TextChangedEventArgs e) =>
            listView.ItemsSource = _homeViewModel.Search(e.NewTextValue);

        private void LabelNameTapFunc()
        {
            LabelName.GestureRecognizers.Add(
                new TapGestureRecognizer() {
                    Command = new Command(() => {
                        _homeViewModel.ActiveSearchLabelStyle(LabelName);
                        _homeViewModel.DisableSearchLabelStyle(LabelName);
                        listView.ItemsSource = _homeViewModel.Search(searchBar.Text);
                    })
                });
        }

        private void LabelSubjectTapFunc()
        {
            LabelSubject.GestureRecognizers.Add(new TapGestureRecognizer() {
                Command = new Command(() => {
                    _homeViewModel.ActiveSearchLabelStyle(LabelSubject);
                    _homeViewModel.DisableSearchLabelStyle(LabelSubject);
                    listView.ItemsSource = _homeViewModel.Search(searchBar.Text);
                })
            });
        }

        private void LabelDateTapFunc()
        {
            LabelDate.GestureRecognizers.Add(new TapGestureRecognizer() {
                Command = new Command(() => {
                    _homeViewModel.ActiveSearchLabelStyle(LabelDate);
                    _homeViewModel.DisableSearchLabelStyle(LabelDate);
                    listView.ItemsSource = _homeViewModel.Search(searchBar.Text);
                })
            });
        }

        private void TapEvent() => TapViewModel.GetTapGestureRecognizer(_tappedFrame);
    }
}