using System;
using System.Threading.Tasks;
using Labs.Data;
using Labs.Helpers;
using Labs.ViewModels.Creators;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Labs.Views.Creators
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TypeEntryCreatingPage
    {
        private bool _settingsIsVisible;
        private uint _heightMax = 0;
        public TypeEntryCreatingPage(string questionId, string testId)
        {
            InitializeComponent();
            InitializeAsync(questionId, testId);
        }

        private async void InitializeAsync(string questionId, string testId)
        {
            await Task.Run(() => {
                var creatorViewModel = new TestCreatorViewModel(Repository.Type.Entry, questionId, testId)
                {
                    Page = this
                };
                Device.BeginInvokeOnMainThread(()=>{ BindingContext = creatorViewModel; });
            });
        }

        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e) =>
            ((ListView)sender).SelectedItem = null;
    }
}