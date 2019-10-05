using System;
using Labs.Helpers;
using Labs.Interfaces;
using Labs.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Labs.Views.TestPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResultPage : ContentPage
    {
        public const string Check = "Check";
        private bool _isClickAble = true;
        private readonly ISettings _model;
        public ResultPage(ISettings settings)
        {
            InitializeComponent();
            LabelPrice.Text = "/" + settings.TotalPrice;
            LabelCount.Text = "/" + settings.TotalCount;
            _model = settings;
            BindingContext = _model;
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            if (_isClickAble)
            {
                _model.Price = "0";
                _model.TotalCount = "0";
                MessagingCenter.Send<Page>(this, Check);
                MessagingCenter.Send<object>(this, TestPage.ReturnPages);
                _isClickAble = false;
            }
        }

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () => {
                var result = await DisplayAlert(AppResources.Warning, AppResources.Escape, AppResources.Yes, 
                    AppResources.No);
                if (!result) return;
                await Navigation.PopModalAsync(true);
            });

            return true;
        }

        private void BackButton_OnClicked(object sender, EventArgs e) => OnBackButtonPressed();
    }
}