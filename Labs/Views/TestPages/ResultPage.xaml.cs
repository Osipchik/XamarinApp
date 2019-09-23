using System;
using Labs.Helpers;
using Labs.Models;
using Labs.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Labs.Views.TestPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResultPage : ContentPage
    {
        private bool _isClickAble = true;
        private readonly TestModel _model;
        public ResultPage(TestModel testModel)
        {
            InitializeComponent();
            _model = testModel;
            BindingContext = _model;
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            if (_isClickAble) {
                _model.Price = 0;
                _model.RightAnswers = 0;
                MessagingCenter.Send<Page>(this, Constants.Check);
                MessagingCenter.Send<object>(this, Constants.ReturnPages);
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