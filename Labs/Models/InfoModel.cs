using System.Collections.ObjectModel;
using System.IO;
using Xamarin.Forms;

namespace Labs.Models
{
    public class InfoModel
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
        public string Date { get; set; }

        public Color TitleColor { get; set; }
        public Color DetailColor { get; set; }
        public Color DateColor { get; set; }
    }
}
