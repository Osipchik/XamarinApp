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
            ReadFileAsync();
            AddDeleteToolBarItem();
        }

        private void AddDeleteToolBarItem()
        {
            var item = new ToolbarItem
            {
                Text = AppResources.Delete,
                Order = ToolbarItemOrder.Secondary
            };
            item.Clicked += ItemDelete_OnClicked;
            ToolbarItems.Add(item);
        }

        private async void ReadFileAsync()
        {
            await Task.Run(()=>{
                using (var reader = new StreamReader(Path.Combine(_path, _fileName)))
                {
                    Coast.Text = reader.ReadLine();
                    Answer.Text = reader.ReadLine();
                    Question.Text = reader.ReadLine();
                }
            });
        }

        private async void Coast_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (PageHelper.CheckCoast(Coast.Text) == false)
                await DisplayAlert("Warning", "Invalid coast value", "cancel");
        }

        private async void SaveButton_OnClicked(object sender, EventArgs e)
        {
            if (Question.Text == null)
            {
                await DisplayAlert(AppResources.Warning, AppResources.WarningQuestion, AppResources.Cancel);
                return;
            }
            if (Answer.Text == null)
            {
                await DisplayAlert(AppResources.Warning, AppResources.WarningAnswer, AppResources.Cancel);
                return;
            }
            if (PageHelper.CheckCoast(Coast.Text) == false)
            {
                await DisplayAlert(AppResources.Warning, AppResources.WarningPrice, AppResources.Cancel);
                return;
            }

            var toSaveStrings = new string[3];
            toSaveStrings[0] = Coast.Text;
            toSaveStrings[1] = Answer.Text;
            toSaveStrings[2] = Question.Text;

            string fileName = await DirectoryHelper.GetFileNameAsync("Entry", _path, _fileName);

            // TODO чтобы не вылетало при первом сохранении
            try { await Task.Run(() => File.WriteAllLines(fileName, toSaveStrings)); }
            catch (Exception exception)
            {
                // ignored
            }

            MessagingCenter.Send<Page>(this, "CreatorListUpLoad");
            //MessagingCenter.Send<Page>(this, "StartInfoUpLoad");
            await Navigation.PopAsync(true);
        }

        private async void ItemDelete_OnClicked(object sender, EventArgs e)
        {
            await Task.Run(() => File.Delete(Path.Combine(_path, _fileName)));
            MessagingCenter.Send<Page>(this, "CreatorListUpLoad");
            MessagingCenter.Send<Page>(this, "StartInfoUpLoad");
            await Navigation.PopAsync(true);
        }
    }
}