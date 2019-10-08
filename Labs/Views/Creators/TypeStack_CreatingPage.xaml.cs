using System;
using System.Threading.Tasks;
using FFImageLoading.Forms;
using Labs.Data;
using Labs.Helpers;
using Labs.ViewModels;
using Labs.ViewModels.Creators;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Labs.Views.Creators
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TypeStackCreatingPage
    {
        private TestCreatorViewModel _creatorViewModel;
        private bool _settingsIsVisible = true;
        private uint _heightMax;
        public TypeStackCreatingPage(string questionId, string testId)
        {
            InitializeComponent();
            InitializeAsync(questionId, testId);
        }

        private async void InitializeAsync(string questionId, string testId)
        {
            await Task.Run(() => {
                _creatorViewModel = new TestCreatorViewModel(Repository.Type.Stack, questionId, testId)
                {
                    GridButtons = GridButtons, Page = this
                };
                Device.BeginInvokeOnMainThread(() => BindingContext = _creatorViewModel);
            });
        }

        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e) =>
            ((ListView)sender).SelectedItem = null;

        private void ListView_OnItemTapped(object sender, ItemTappedEventArgs e) => 
            _creatorViewModel.FrameViewModel.TapEvent(e.ItemIndex);

        private void ChooseItemsToDelete_OnClicked(object sender, EventArgs e) =>
            ChooseItems(FrameViewModel.Mode.ItemDelete);

        private void ChooseItems(FrameViewModel.Mode modificator)
        {
            GridButtons.IsVisible = true;
            SetActionToGridButtons(ImageButtonAccept, modificator);
        }

        private void SetActionToGridButtons(CachedImage accept, FrameViewModel.Mode modificator)
        {
            _creatorViewModel.FrameViewModel.Modificator = modificator;
            _creatorViewModel.FrameViewModel.DisableAllAsync();
            if (modificator < 0) {
                accept.Opacity = 1;
                accept.Source = "CheckedRed.png";
            }
            else {
                accept.Opacity = 0.3;
                accept.Source = (ImageSource)Application.Current.Resources["CheckedButtonImage"];
            }
        }
    }
}