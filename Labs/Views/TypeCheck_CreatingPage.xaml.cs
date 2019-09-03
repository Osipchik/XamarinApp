using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Labs.Helpers;
using Labs.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace Labs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TypeCheckCreatingPage
    {
        private readonly string _path;
        private readonly string _fileName;
        
        public TypeCheckCreatingPage(string path)
        {
            InitializeComponent();
            _path = path;
        }

        public TypeCheckCreatingPage(string path, string fileName) : this(path)
        {
            _fileName = fileName;
            ReadFileAsync();
            ItemClear.Text = AppResources.Delete;
        }

        private async void ReadFileAsync()
        {
            await Task.Run(()=>
            {
                using (var reader = new StreamReader(Path.Combine(_path, _fileName)))
                {
                    int count = 0;
                    Coast.Text = reader.ReadLine();
                    string isChecked = reader.ReadLine();
                    Question.Text = reader.ReadLine();
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (isChecked[count] == '1') NewRow(line, true);
                        else NewRow(line);
                        count++;
                    }
                }
            });
        }

        private Grid NewRow(string text = "", bool isChecked = false)
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
                ColumnSpacing = 0
            };

            var checkBox = CommonPageHelper.GetCheckBox(isChecked);

            var edit = CommonPageHelper.GetEditor(text, AppResources.TypeCheckEditorPlaceh);
            edit.WidthRequest = 294 * (Device.info.PixelScreenSize.Width) / 1080;

            var deleteButton = CommonPageHelper.GetDeleteButton();
            deleteButton.Clicked += DeleteButton_Clicked;

            grid.Children.Add(checkBox, 0, 0);
            grid.Children.Add(edit, 1, 0);
            grid.Children.Add(deleteButton, 2, 0);

            return grid;
        }

        private async void TextArea_Button_OnClicked(object sender, EventArgs e)
        {
            StackLayoutContent.Children.Add(await Task.Run(() => NewRow()));
        }

        private void DeleteButton_Clicked(object sender, EventArgs e)
        {
            var gridElement = ((ImageButton) sender).Parent;
            StackLayoutContent.Children.RemoveAt(StackLayoutContent.Children.IndexOf(gridElement));
            //int index = StackLayoutContent.Children.IndexOf(gridElement);
            //StackLayoutContent.Children.RemoveAt(index);
        }

        private List<string> GetToSaveStrings()
        {
            var toSaveStrings = new List<string> { Question.Text };
            var toSaveCheck = "";

            foreach (var element in StackLayoutContent.Children)
            {
                var editorText = ((Editor)((Grid)element).Children[1]).Text;
                if (editorText == null) return null;

                if (((CheckBox)((Grid)element).Children[0]).IsChecked) toSaveCheck += "1";
                else toSaveCheck += "0";

                toSaveStrings.Add(editorText);
            }
            toSaveStrings.Insert(0, toSaveCheck);
            toSaveStrings.Insert(0, Coast.Text ?? "0");

            return toSaveStrings;
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
                await DisplayAlert(AppResources.Warning, AppResources.WarningTextEmpty, AppResources.Cancel);
                return;
            }
            if (CommonPageHelper.CheckCoast(Coast.Text) == false)
            {
                await DisplayAlert(AppResources.Warning, AppResources.WarningPrice, AppResources.Cancel);
                return;
            }

            var toSaveStrings = await Task.Run(GetToSaveStrings);

            if (toSaveStrings == null)
            {
                await DisplayAlert(AppResources.Warning, AppResources.WarningAnswer, AppResources.Cancel);
                return;
            }

            // TODO чтобы не вылетало при первом сохранении
            try
            {
                await Task.Run((() =>File.WriteAllLines(CommonPageHelper.GetFileName("Check", _path, _fileName), toSaveStrings)));
            }
            catch (Exception exception)
            {
                // ignored
            }

            MessagingCenter.Send<Page>(this, Constants.CreatorListUpLoad);
            //MessagingCenter.Send<Page>(this, "StartInfoUpLoad");
            await Navigation.PopAsync(true);
        }

        private async void ItemClear_OnClicked(object sender, EventArgs e)
        {
            if (_fileName != null)
            {
                await Task.Run(() => File.Delete(Path.Combine(_path, _fileName)));
                MessagingCenter.Send<Page>(this, Constants.CreatorListUpLoad);
                await Navigation.PopAsync(true);
            }
            else
            {
                Coast.Text = "0";
                Question.Text = string.Empty;
                StackLayoutContent.Children.Clear();
            }
        }

        private async void Coast_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (CommonPageHelper.CheckCoast(Coast.Text) == false)
                await DisplayAlert("Warning", "Invalid coast value", "cancel");
        }
    }
}