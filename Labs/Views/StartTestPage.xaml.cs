﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Labs.Helpers;
using Labs.Models;
using Labs.Resources;
using Labs.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Labs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartTestPage
    {
        private readonly string _path;
        private readonly TestViewModel _test;
        private int _count;

        public StartTestPage(string path)
        {
            InitializeComponent();

            _path = path;
            _test = new TestViewModel(path);
            FillInfo();
            Subscribe();
        }

        private async void FillInfo() => GetSettings(await Task.Run(() => _test.GetSettingsDictionary()));
        private void GetSettings(Dictionary<string, string> collection)
        {
            LabelName.Text = collection["name"];
            LabelSubject.Text = collection["subject"];
            LabelPrice.Text = collection["price"];
            LabelTime.Text = collection["time"];
        }

        private void Subscribe()
        {
            MessagingCenter.Subscribe<Page>(this, Constants.StartPageCallBack,
                (sender) => { StartPageCallBack(); });
        }

        private void StartPageCallBack()
        {
            FillInfo();
            _count = 0;
            ChangeButtonStyle_OnCallBack(ChangeButton);
            ChangeButtonStyle_OnCallBack(StartButton);
            MessagingCenter.Send<Page>(this, Constants.HomeListUpload);
        }

        private async void ChangeButton_OnClicked(object sender, EventArgs e)
        {
            ChangeButtonStyle_OnClick(ChangeButton);
            if (_count != 0) return;
            _count++;
            await Navigation.PushAsync(new CreatorPage(_path));
        }

        private async void StartButton_OnClicked(object sender, EventArgs e)
        {
            ChangeButtonStyle_OnClick(StartButton);
            if (_count != 0) return;
            _count++;
            await Navigation.PushModalAsync(new TestPage(_path));
        }

        private void ChangeButtonStyle_OnClick(Button button)
        {
            button.BackgroundColor = Color.FromHex(Constants.ColorMaterialBlue);
            button.TextColor = Color.White;
        }

        private void ChangeButtonStyle_OnCallBack(Button button)
        {
            button.BackgroundColor = Color.White;
            button.TextColor = Color.FromHex(Constants.ColorMaterialBlue);
        }
    }
}