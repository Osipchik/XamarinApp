using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Labs.Helpers;
using Labs.MainPages;
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
        // TODO: fix this
        private readonly string _folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        private readonly DirectoryInfo _dirInfoTests;
        private readonly InfoCollectionViewModel _directoryInfoCollection;

        [Obsolete]
        public HomePage()
        {
            InitializeComponent();

            //var anim = new AnimationView
            //{
            //    Animation = "CheckLoad.json",
            //    Loop = false,
            //    HeightRequest = 100,
            //    WidthRequest = 100,
            //    AutoPlay = true
            //};
            //anim.OnClick += AnimView_OnOnClick;

            //stackLayout.Children.Add(anim);
            _dirInfoTests = new DirectoryInfo(Path.Combine(_folderPath, Constants.TestFolder));
            _directoryInfoCollection = new InfoCollectionViewModel();

            ListUploadAsync();
            Subscribe();

            LabelNameTapFunc();
            LabelSubjectTapFunc();
            LabelDateTapFunc();
        }

        private void ListUploadAsync() =>
            listView.ItemsSource = _directoryInfoCollection.GetDirectoryInfo(new DirectoryInfo(_dirInfoTests.FullName));
        

        private void Subscribe()
        {
            MessagingCenter.Subscribe<Page>(
                this, Constants.HomeListUpload, (sender) => { ListUploadAsync(); });
        }


        // TODO: с этим надо что-то сделать
        //private void AnimView_OnOnClick(object sender, EventArgs e)
        //{
        //    var anim = (AnimationView)sender;
        //    anim.Progress = 0.0f;
        //    anim.IsPlaying = true;
        //    anim.Progress = 0.0f;
        //}


        private async void ListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            await Navigation.PushAsync(
                new StartTestPage(Path.Combine(_dirInfoTests.FullName, _directoryInfoCollection.InfosCollection[e.ItemIndex].Name)));
        }

        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = (ListView)sender;
            item.SelectedItem = null;
        }

        private void SearchBar_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var keyword = searchBar.Text;

            if (keyword == ""){
                DisableGridButtonsAsync();
                return;
            }

            GridButtons.IsVisible = true;
            if (LabelName.TextColor == Color.FromHex("#03A9F4")) SearchNameAsync(keyword);
            else if (LabelSubject.TextColor == Color.FromHex("#03A9F4")) SearchSubjectAsync(keyword);
            else if (LabelDate.TextColor == Color.FromHex("#03A9F4")) SearchDateAsync(keyword);
        }

        private async void DisableGridButtonsAsync()
        {
            GridButtons.IsVisible = false;
            await Task.Run(() =>
            {
                foreach (var collectionView in _directoryInfoCollection.InfosCollection)
                {
                    collectionView.TitleColor =
                        collectionView.DetailColor =
                            collectionView.DateColor =
                                Color.FromHex("#757575");
                }

                listView.ItemsSource = _directoryInfoCollection.InfosCollection;
            });
        }

        private async void SearchNameAsync(string keyword)
        {
            IEnumerable<InfoModel> searchResult = from title
                                                       in _directoryInfoCollection.InfosCollection
                                                       where title.Title.ToLower().Contains(keyword.ToLower())
                                                       select title;
          
            listView.ItemsSource = await Task.Run(()=>ColorName(searchResult));
        }

        private IEnumerable<InfoModel> ColorName(IEnumerable<InfoModel> nameCollectionViews)
        {
            var collectionViews = nameCollectionViews as InfoModel[] ?? nameCollectionViews.ToArray();
            foreach (var collectionView in collectionViews)
            {
                collectionView.TitleColor = Color.FromHex("#03A9F4");

                collectionView.DetailColor =
                    collectionView.DateColor =
                        Color.FromHex("#757575");
            }

            return collectionViews;
        }

        private async void SearchSubjectAsync(string keyword)
        {
            IEnumerable<InfoModel> searchResult = from detail
                                                            in _directoryInfoCollection.InfosCollection
                                                            where detail.Detail.ToLower().Contains(keyword.ToLower())
                                                            select detail;

            listView.ItemsSource = await Task.Run(()=>ColorSubject(searchResult));
        }

        private IEnumerable<InfoModel> ColorSubject(IEnumerable<InfoModel> subjectCollectionViews)
        {
            var collectionViews = subjectCollectionViews as InfoModel[] ?? subjectCollectionViews.ToArray();
            foreach (var collectionView in collectionViews)
            {
                collectionView.DetailColor = Color.FromHex("#03A9F4");

                collectionView.TitleColor =
                    collectionView.DateColor =
                        Color.FromHex("#757575");
            }

            return collectionViews;
        }

        private async void SearchDateAsync(string keyword)
        {
            IEnumerable<InfoModel> searchResult = from date
                                                            in _directoryInfoCollection.InfosCollection
                                                            where date.Date.ToLower().Contains(keyword.ToLower())
                                                            select date;

            listView.ItemsSource = await Task.Run(()=> ColorDate(searchResult));
        }

        private IEnumerable<InfoModel> ColorDate(IEnumerable<InfoModel> dateCollectionViews)
        {
            var collectionViews = dateCollectionViews as InfoModel[] ?? dateCollectionViews.ToArray();
            foreach (var collectionView in collectionViews)
            {
                collectionView.DateColor = Color.FromHex("#03A9F4");

                collectionView.TitleColor =
                    collectionView.DetailColor =
                        Color.FromHex("#757575");
            }

            return collectionViews;
        }


        private void LabelNameTapFunc()
        {
            LabelName.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    LabelName.FontSize = 16;
                    LabelSubject.FontSize = LabelDate.FontSize = 14;

                    LabelName.TextColor = Color.FromHex("#03A9F4");
                    LabelSubject.TextColor = LabelDate.TextColor = Color.FromHex("#757575");
                    if (GridButtons.IsVisible) SearchNameAsync(searchBar.Text);
                })
            });
        }

        private void LabelSubjectTapFunc()
        {
            LabelSubject.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    LabelSubject.FontSize = 16;
                    LabelName.FontSize = LabelDate.FontSize = 14;

                    LabelSubject.TextColor = Color.FromHex("#03A9F4");
                    LabelName.TextColor = LabelDate.TextColor = Color.FromHex("#757575");
                    if (GridButtons.IsVisible) SearchSubjectAsync(searchBar.Text);
                })
            });
        }

        private void LabelDateTapFunc()
        {
            LabelDate.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    LabelDate.FontSize = 16;
                    LabelSubject.FontSize = LabelName.FontSize = 14;

                    LabelDate.TextColor = Color.FromHex("#03A9F4");
                    LabelSubject.TextColor = LabelName.TextColor = Color.FromHex("#757575");
                    if (GridButtons.IsVisible) SearchDateAsync(searchBar.Text);
                })
            });
        }
    }
}