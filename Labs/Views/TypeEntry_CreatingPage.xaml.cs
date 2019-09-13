using System;
using Labs.Helpers;
using Labs.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Labs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TypeEntryCreatingPage
    {
        private readonly string _path;
        private readonly string _fileName;
        private readonly CheckTypeCreatorViewModel _viewModel;
        private bool _settingsIsVisible;
        private uint _heightMax = 0;
        public TypeEntryCreatingPage(string path, string fileName = "")
        {
            InitializeComponent();

            _fileName = fileName;
            _path = path;
            _viewModel = new CheckTypeCreatorViewModel(path, fileName, this);

            BindingContext = _viewModel;
            SettingsLayout.BindingContext = _viewModel.GetSettingsModel;
        }

        private void HideOrShowAsync_OnClicked(object sender, EventArgs e)
        {
            _settingsIsVisible = !_settingsIsVisible;
            if (_heightMax == 0) _heightMax = (uint)SettingsLayout.Height;
            FrameAnimation.RunShowOrHideAnimation(SettingsLayout, _heightMax, 0, _settingsIsVisible);
        }
    }
}