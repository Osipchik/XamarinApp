﻿using System;
using System.ComponentModel;
using System.IO;
using Labs.Helpers;
using Labs.Models;
using Xamarin.Forms;

namespace Labs.Views
{
    [DesignTimeVisible(false)]
    public partial class MainPage
    {
        [Obsolete]
        public MainPage()
        {
            InitializeComponent();
            IsPresented = false;
            
            ShowHomePage();
            BindingContext = this;
            ListViewDetail.ItemsSource = MasterDetail.GetDetailItems();
            Subscribe();
        }

        private void Subscribe() =>
            MessagingCenter.Subscribe<Page>(this, Constants.UploadTitles, (sender) => UploadTitles());
        

        private void UploadTitles()
        {
            //ListViewDetail.ItemsSource = null;
            ListViewDetail.ItemsSource = MasterDetail.GetDetailItems();
        }

        private string[] GetPaths()
        {
            var appPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string[] paths = {
                Path.Combine(appPath, Constants.TestFolder),
                Path.Combine(appPath, Constants.TempFolder)
            };

            return paths;
        }

        private void CheckFolders(params string[] paths)
        {
            if (!Directory.Exists(paths[0])) Directory.CreateDirectory(paths[0]);
            if (!Directory.Exists(paths[1])) Directory.CreateDirectory(paths[1]);
        }

        [Obsolete]
        private void ShowHomePage()
        {
            CheckFolders(GetPaths());
            Detail = new NavigationPage(new HomePage());
        }

        [Obsolete]
        private void ListViewDetail_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            switch (e.ItemIndex)
            {
                case 0:
                    Detail = new NavigationPage(new HomePage());
                    IsPresented = false;
                    break;
                case 1:
                    var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                    Detail = new NavigationPage(new CreatorPage(Path.Combine(path, Constants.TempFolder)));
                    IsPresented = false;
                    break;
                case 2:
                    Detail = new NavigationPage(new SettingsPage());
                    IsPresented = false;
                    break;
            }
        }

        private void ListViewDetail_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = (ListView)sender;
            item.SelectedItem = null;
        }
    }
}