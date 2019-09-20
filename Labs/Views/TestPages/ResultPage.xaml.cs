using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Labs.Helpers;
using Labs.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Labs.Views.TestPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResultPage : ContentPage
    {
        private bool isClickAble = true;
            public ResultPage(TestModel testModel)
        {
            InitializeComponent();
            BindingContext = testModel;
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            if (isClickAble)
            {
                MessagingCenter.Send<Page>(this, Constants.Check);
                isClickAble = false;
            }
        }
    }
}