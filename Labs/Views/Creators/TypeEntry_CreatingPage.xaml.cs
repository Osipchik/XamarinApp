using System;
using Labs.Helpers;
using Labs.ViewModels.Creators;
using Xamarin.Forms.Xaml;

namespace Labs.Views.Creators
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TypeEntryCreatingPage
    {
        private bool _settingsIsVisible;
        private uint _heightMax = 0;
        public TypeEntryCreatingPage(string path, string fileName = "")
        {
            InitializeComponent();

            var entryViewModel = new EntryTypeCreatorViewModel(path, fileName, this);

            BindingContext = entryViewModel;
            SettingsLayout.BindingContext = entryViewModel.GetSettingsModel;
        }

        private void HideOrShowAsync_OnClicked(object sender, EventArgs e)
        {
            _settingsIsVisible = !_settingsIsVisible;
            if (_heightMax == 0) _heightMax = (uint)SettingsLayout.Height;
            FrameAnimation.RunShowOrHideAnimation(SettingsLayout, _heightMax, 0, _settingsIsVisible);
        }
    }
}