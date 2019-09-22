using Labs.Resources;
using Xamarin.Forms;

namespace Labs.ViewModels.Tests
{
    public class TestViewModel
    {
        private readonly Page _page;

        public TestViewModel(Page page)
        {
            _page = page;
        }

        public bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () => {
                var result = await _page.DisplayAlert(AppResources.Warning, AppResources.Escape, AppResources.Yes, AppResources.No);
                if (!result) return;
                await _page.Navigation.PopModalAsync(true);
            });

            return true;
        }
    }
}
