using Rectangle = Xamarin.Forms.Rectangle;

namespace Labs.Models
{
    public class TimerModel
    {
        public double Progress { get; set; }
        public string Time { get; set; }
        public Rectangle Bounds { get; set; }
    }
}
