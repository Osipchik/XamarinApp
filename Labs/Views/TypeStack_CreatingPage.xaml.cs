using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Labs.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace Labs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TypeStackCreatingPage
    {
        private readonly string _path;
        private readonly string _fileName;

        public TypeStackCreatingPage(string path)
        {
            InitializeComponent();
            _path = path;
        }

        public TypeStackCreatingPage(string path, string fileName) : this(path)
        {
            _fileName = fileName;
            ReadFileAsync();
            AddDeleteToolBarItem();
        }

        private async void ReadFileAsync()
        {
            await Task.Run(() =>
            {
                using (var reader = new StreamReader(Path.Combine(_path, _fileName)))
                {
                    Coast.Text = reader.ReadLine();
                    Question.Text = reader.ReadLine();
                    Question.Text = reader.ReadLine();
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        NewRow(line, reader.ReadLine());
                    }
                }
            });
        }

        private void AddDeleteToolBarItem()
        {
            var item = new ToolbarItem
            {
                Text = "Delete",
                Order = ToolbarItemOrder.Secondary
            };
            item.Clicked += ItemDelete_OnClicked;
            ToolbarItems.Add(item);
        }

        private async void ItemDelete_OnClicked(object sender, EventArgs e)
        {
            await Task.Run(() => File.Delete(Path.Combine(_path, _fileName)));
            MessagingCenter.Send<Page>(this, "CreatorListUpLoad");
            await Navigation.PopAsync(true);
        }

        private void NewRow(string textFirst = "", string textSecond = "")
        {
            var grid = new Grid
            {
                RowDefinitions = { new RowDefinition { Height = GridLength.Auto } },
                ColumnDefinitions =
                {
                    new ColumnDefinition {Width = GridLength.Auto},
                    new ColumnDefinition {Width = GridLength.Auto},
                    new ColumnDefinition {Width = GridLength.Auto,}
                },
                ColumnSpacing = 0,
            };

            var textAreaFirst = CommonPageHelper.GetEditor(textFirst, AppResources.Text);
            textAreaFirst.WidthRequest = 160 * (Device.info.PixelScreenSize.Width) / 1080;

            var textAreaSecond = CommonPageHelper.GetEditor(textSecond, AppResources.Answer);
            textAreaSecond.WidthRequest = 160 * (Device.info.PixelScreenSize.Width) / 1080;

            var deleteButton = CommonPageHelper.GetDeleteButton();
            deleteButton.Clicked += DeleteButton_Clicked;

            grid.Children.Add(textAreaFirst, 0, 0);
            grid.Children.Add(textAreaSecond, 1, 0);
            grid.Children.Add(deleteButton, 2, 0);
            StackLayoutContent.Children.Add(grid);
        }

        private void AddLine_Button_OnClicked(object sender, EventArgs e)
        {
            NewRow();
        }

        private void DeleteButton_Clicked(object sender, EventArgs e)
        {
            var gridElement = ((ImageButton)sender).Parent;
            int index = StackLayoutContent.Children.IndexOf(gridElement);
            StackLayoutContent.Children.RemoveAt(index);
        }

        private async void SaveButton_OnClicked(object sender, EventArgs e)
        {
            if (Question.Text == null)
            {
                await DisplayAlert(AppResources.Warning, AppResources.WarningQuestion, AppResources.Cancel);
                return;
            }
            if (StackLayoutContent.Children.Count == 0)
            {
                await DisplayAlert(AppResources.Warning, AppResources.Text, AppResources.Cancel);
                return;
            }
            if (CommonPageHelper.CheckCoast(Coast.Text) == false)
            {
                await DisplayAlert(AppResources.Warning, AppResources.WarningPrice, AppResources.Cancel);
                return;
            }
            var toSaveStrings = GetToSaveStrings();
            if (toSaveStrings == null)
            {
                await DisplayAlert(AppResources.Warning, AppResources.WarningAnswer, AppResources.Cancel);
                return;
            }

            // TODO чтобы не вылетало при первом сохранении
            try { File.WriteAllLines(CommonPageHelper.GetFileName("Stack", _path, _fileName), toSaveStrings); }
            catch (Exception exception)
            {
                // ignored
            }

            MessagingCenter.Send<Page>(this, "CreatorListUpLoad");
            MessagingCenter.Send<Page>(this, "StartInfoUpLoad");
            await Navigation.PopAsync(true);
        }

        private List<string> GetToSaveStrings()
        {
            var toSaveStrings = new List<string> { Question.Text };

            foreach (var element in StackLayoutContent.Children)
            {
                var editorTextFirst = ((Editor)((Grid)element).Children[0]).Text;
                if (editorTextFirst == "") return null;

                var editorTextSecond = ((Editor)((Grid)element).Children[1]).Text;
                if (editorTextSecond == "") return null;

                toSaveStrings.Add(editorTextFirst);
                toSaveStrings.Add(editorTextSecond);
            }
            toSaveStrings.Insert(0, "0");
            toSaveStrings.Insert(0, Coast.Text ?? "0");

            return toSaveStrings;
        }

        private async void Coast_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (CommonPageHelper.CheckCoast(Coast.Text) == false)
                await DisplayAlert("Warning", "Invalid coast value", "cancel");
        }
    }
}