using System;
using System.IO;
using System.Threading.Tasks;
using Labs.Models;
using Labs.Resources;
using Labs.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Labs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreatorPage
    {
        private bool _tableVisible = true;

        private static string _path;
        private DirectoryInfo _directory;
        private InfoCollectionViewModel _directoryInfoCollection;
        private readonly string _subDirectory;


        public CreatorPage(string subDirectory, string folder = "")
        {
            InitializeComponent();

            _path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            _subDirectory = subDirectory;
            if (folder == "") InitTemp();
            else InitExist(folder);

            FillListViewFilesAsync();
            Subscribe();
        }

        private async void InitTemp()
        {
            _directory = new DirectoryInfo(Path.Combine(_path, _subDirectory));
            _directoryInfoCollection = new InfoCollectionViewModel();

            await Task.Run(()=>_directory.Create());
            ItemSave.Clicked += SaveNewTest;
            ItemClear.Clicked += Clear;
        }

        private async void InitExist(string folder)
        {
            _directory = new DirectoryInfo(Path.Combine(_path, _subDirectory, folder));
            _directoryInfoCollection = new InfoCollectionViewModel();

            await Task.Run(FillSettings);

            ItemSave.Clicked += SaveExistingTest;
            ItemClear.Clicked += DeleteAsync;
            ItemClear.Text = AppResources.Delete;
        }

        private void Picker_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedIndex = ((Picker)sender).SelectedIndex;
            if (selectedIndex == 0) stepper.Maximum = 5;
            else if (selectedIndex == 1) stepper.Maximum = 180;
            else stepper.Maximum = 120;
        }

        private async void SettingsButton_OnClickedAsync(object sender, EventArgs e)
        {
            var buttonWidthMin = ButtonSettings.Text.Length * 11;
            if (_tableVisible == false)
            {
                ButtonSettings.BackgroundColor = Color.FromHex("#039BE5");
                ButtonSettings.TextColor = Color.White;
                await Task.Run(() =>
                {
                    new Animation((d) => ((Button)sender).WidthRequest = d, ButtonSettings.Width, 350).Commit(
                        this, "ButtonExpand", 16, 700, Easing.CubicInOut);
                });
                await Task.Run(() =>
                {
                    new Animation((d) => SettingsTableView.HeightRequest = d, 0, 400).Commit(
                        this, "Expand", 16, 700, Easing.SinInOut);
                    _tableVisible = true;
                });
            }
            else
            {
                ButtonSettings.BackgroundColor = Color.White;
                ButtonSettings.TextColor = Color.FromHex("#039BE5");
                await Task.Run(() =>
                {
                    new Animation((d) => ((Button)sender).WidthRequest = d, 350, buttonWidthMin).Commit(
                        this, "ButtonExpand", 16, 1000, Easing.CubicInOut);
                });
                await Task.Run(() =>
                {
                    new Animation((d) => SettingsTableView.HeightRequest = d, 400, 0).Commit(
                        this, "Expand", 60, 1000, Easing.SinInOut);
                    _tableVisible = false;
                });
            }
        }

        private void SwitchCellWhole_OnChanged(object sender, ToggledEventArgs e)
        {
            SwitchCellQuestion.On = !e.Value;
        }

        private void SwitchCellQuestion_OnChanged(object sender, ToggledEventArgs e)
        {
            SwitchCellWhole.On = !e.Value;
        }

        private async void FillListViewFilesAsync()
        {
            _directoryInfoCollection.InfosCollection = await Task.Run(()=> InfoCollection.GetFilesInfo(_directory));

            ListViewFiles.ItemsSource = _directoryInfoCollection.InfosCollection;
        }

        private void FillSettings()
        {
            using (var reader = new StreamReader(Path.Combine(_directory.FullName, "settings.txt")))
            {
                CellName.Text = reader.ReadLine();
                CellSubject.Text = reader.ReadLine();
                if (reader.ReadLine() != "True") SwitchCellWhole.On = false;
                picker.SelectedIndex = int.Parse(reader.ReadLine());
                stepper.Value = int.Parse(reader.ReadLine());
            }
        }

        private void Subscribe()
        {
            MessagingCenter.Subscribe<Page>(
                this,
                "CreatorListUpLoad",
                (sender) => { FillListViewFilesAsync(); });
        }

        private string[] GetSettingsToSave()
        {
            var settings = new string[5];

            if (CellName.Text != null || CellName.Text == "") settings[0] = CellName.Text;
            else{
                return null;
            }
            if (CellSubject.Text != null || CellSubject.Text == "") settings[1] = CellSubject.Text;
            else{
                return null;
            }

            settings[2] = SwitchCellWhole.On.ToString();
            settings[3] = picker.SelectedIndex.ToString();
            settings[4] = LabelTime.Text;

            return settings;
        }

        private async void SaveNewTest(object sender, EventArgs e)
        {
            var settings = GetSettingsToSave();
            if (settings == null){
                await DisplayAlert(AppResources.Warning, AppResources.FillSettings, AppResources.Cancel);
                return;
            }

            if (_directoryInfoCollection.InfosCollection.Count == 0){
                await DisplayAlert(AppResources.Warning, AppResources.CreatorQuestions, AppResources.Cancel);
                return;
            }

            await Task.Run(()=>File.WriteAllLines(Path.Combine(_directory.FullName, "settings.txt"), settings));

            var count = 0;
            var testPath = Path.Combine(_path, "Tests", $"Test_{count}");

            await Task.Run(() =>
            {
                while (Directory.Exists(testPath))
                {
                    count++;
                    testPath = Path.Combine(_path, "Tests", $"Test_{count}");
                }

                _directory.MoveTo(testPath);
            });

            Clear(this, EventArgs.Empty);

            ListViewFiles.ItemsSource = null;
        }

        private async void SaveExistingTest(object sender, EventArgs e)
        {
            var settings = await Task.Run(GetSettingsToSave);
            if (settings == null)
            {
                await DisplayAlert(AppResources.Warning, AppResources.FillSettings, AppResources.Cancel);
                return;
            }

            await Task.Run(()=>File.WriteAllLines(Path.Combine(_directory.FullName, "settings.txt"), settings));
            MessagingCenter.Send<Page>(this, "HomeListUpload");
            MessagingCenter.Send<Page>(this, "StartInfoUpLoad");
            await Navigation.PopAsync(true);
        }

        private void Clear(object sender, EventArgs e)
        {
            if (Directory.Exists(Path.Combine(_path, "Temp"))) _directory.Delete(true);
            _directory.Create();
            CellName.Text = CellSubject.Text = string.Empty;
            ListViewFiles.ItemsSource = null;
        }

        private async void DeleteAsync(object sender, EventArgs e)
        {
            if (await DisplayAlert(AppResources.Warning, AppResources.DeleteAnswer, AppResources.Yes, AppResources.No))
            {
                await Task.Run(() =>
                {
                    _directory.Delete(true);
                });

                await Task.Run(()=>MessagingCenter.Send<Page>(this, "HomeListUpload"));
                await Navigation.PopToRootAsync(true);
            }
        }

        private async void ListViewFiles_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            switch (CommonPageHelper.GetTypeName(_directoryInfoCollection.InfosCollection[e.ItemIndex].Name))
            {
                case "Check":
                    await Navigation.PushAsync(new TypeCheckCreatingPage(_directory.FullName, _directoryInfoCollection.InfosCollection[e.ItemIndex].Name));
                    break;
                case "Stack":
                    await Navigation.PushAsync(new TypeStackCreatingPage(_directory.FullName, _directoryInfoCollection.InfosCollection[e.ItemIndex].Name));
                    break;

                case "Entry":
                    await Navigation.PushAsync(new TypeEntryCreatingPage(_directory.FullName, _directoryInfoCollection.InfosCollection[e.ItemIndex].Name));
                    break;
            }
        }

        private void ListViewFiles_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private async void TypeCheck_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TypeCheckCreatingPage(_directory.FullName));
        }

        private async void TypeEntry_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TypeEntryCreatingPage(_directory.FullName));
        }

        private async void TypeStack_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TypeStackCreatingPage(_directory.FullName));
        }


        protected override bool OnBackButtonPressed()
        {
            if (_subDirectory != "Temp"){
                Device.BeginInvokeOnMainThread(async () =>
                {
                    var result = await DisplayAlert("Caution", "Do you really want to escape this page?", "Yes", "No");
                    if (result){
                        MessagingCenter.Send<Page>(this, "StartInfoUpLoad");
                        Back();
                    }
                });
            }

            return true;
        }

        private async void Back()
        {
            await Navigation.PopAsync(true);
        }
    }
}