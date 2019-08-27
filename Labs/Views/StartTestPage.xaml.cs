using System;
using System.IO;
using System.Threading.Tasks;
using Labs.Models;
using Labs.Resources;
using Labs.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Labs.MainPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartTestPage
    {
        private readonly string _path;
        private readonly string _directoryName;
        private int _count = 0; // чтобы кнопки не нажимались во время загрузки

        public StartTestPage(string path, string directoryName)
        {
            InitializeComponent();

            _path = path;
            _directoryName = directoryName;
            FillInfo();
            Subscribe();
        }

        private void FillInfo()
        {
            using (var reader = new StreamReader(Path.Combine(Path.Combine(_path, _directoryName), "settings.txt")))
            {
                LabelName.Text = reader.ReadLine();
                LabelSubject.Text = AppResources.Subject + " " + reader.ReadLine();
                var time = reader.ReadLine() == "True"
                    ? AppResources.TestSettingsTimeForTest
                    : AppResources.TestSettingsTimeForQuestion;

                var measurement = string.Empty;
                switch (reader.ReadLine())
                {
                    case "0":
                        measurement = "hours";
                        break;
                    case "1":
                        measurement = "minutes";
                        break;
                    case "2":
                        measurement = "seconds";
                        break;
                }

                time += " " + reader.ReadLine() + " ";
                time += measurement;
                LabelTime.Text = time;
            }

            FillCoast();
        }

        private void FillCoast()
        {
            var totalCoast = 0;
            foreach (var coast in InfoCollection.GetFilesInfo(new DirectoryInfo(Path.Combine(_path, _directoryName))))
            {
                totalCoast += int.Parse(coast.Detail);
            }

            LabelCoast.Text = AppResources.MaxMarck + ": " + totalCoast;
        }

        private void Subscribe()
        {
            MessagingCenter.Subscribe<Page>(
                this,
                "StartInfoUpLoad",
                (sender) =>
                {
                    FillInfo();
                    _count = 0;
                });
        }

        private async void ChangeButton_OnClicked(object sender, EventArgs e)
        {
            if (_count != 0) return;
            _count++;
            await Navigation.PushAsync(new CreatorPage("Tests", _directoryName));
        }

        private async void StartButton_OnClicked(object sender, EventArgs e)
        {
            if (_count != 0) return;
            _count++;
            await Navigation.PushModalAsync(new TestPage(Path.Combine(_path, _directoryName)));
        }
    }
}