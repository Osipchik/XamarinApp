using System.Collections.ObjectModel;
using System.IO;
using Xamarin.Forms;

namespace Labs.Models
{
    public class InfoCollection
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
        public string Date { get; set; }

        public Color TitleColor { get; set; }
        public Color DetailColor { get; set; }
        public Color DateColor { get; set; }

        public static ObservableCollection<InfoCollection> GetFilesInfo(DirectoryInfo directoryInfo)
        {
            var infoList = new ObservableCollection<InfoCollection>();
            var files = directoryInfo.GetFiles();
            foreach (var info in files)
            {
                if (info.Name == "settings.txt") continue;
                string title, coast;
                using (var reader = new StreamReader(info.FullName))
                {
                    coast = reader.ReadLine();
                    reader.ReadLine();
                    title = reader.ReadLine();
                }

                infoList.Add(new InfoCollection
                {
                    Name = info.Name,
                    Title = title,
                    Detail = coast,
                    Date = info.CreationTime.ToShortDateString()
                });
            }

            return infoList;
        }

        public static ObservableCollection<InfoCollection> GetDirectoryInfo(DirectoryInfo directoryInfo)
        {
            var infoList = new ObservableCollection<InfoCollection>();
            foreach (var dirInfo in directoryInfo.GetDirectories())
            {
                string title, detail;
                using (var reader = new StreamReader(Path.Combine(dirInfo.FullName, "settings.txt")))
                {
                    title = reader.ReadLine();
                    detail = reader.ReadLine();
                }

                infoList.Add(new InfoCollection
                {
                    Name = dirInfo.Name,
                    Title = title,  //Название теста
                    Detail = detail,//Предмет
                    Date = dirInfo.CreationTime.ToShortDateString()
                });
            }

            return infoList;
        }
    }
}
