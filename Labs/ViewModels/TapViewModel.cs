using Labs.Helpers;
using Xamarin.Forms;

namespace Labs.ViewModels
{
    public class TapViewModel
    {
        public static void GetTapGestureRecognizer(Frame frame, int modificator = 0)
        {
            if (frame.BorderColor == Constants.ColorMaterialRed) {
                frame.BorderColor = Constants.ColorMaterialGray;
            }
            else if (frame.BorderColor == Constants.ColorMaterialBlue) {
                frame.BorderColor = Constants.ColorMaterialGray;
            }
            else switch (modificator) {
                case -1:
                    frame.BorderColor = Constants.ColorMaterialRed;
                    break;
                case 0:
                    frame.BorderColor = Constants.ColorMaterialBlue;
                    break;
                case 1:
                    frame.BorderColor = Constants.ColorMaterialGreen;
                    break;
            }
        }
    }
}
