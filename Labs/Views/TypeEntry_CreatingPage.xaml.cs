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
        private FrameAnimation _animation;
        public TypeEntryCreatingPage(string path, string fileName = "")
        {
            InitializeComponent();

            _fileName = fileName;
            _path = path;
            _animation = new FrameAnimation( 0, (uint)SettingsLayout.Height, 0);
            _viewModel = new CheckTypeCreatorViewModel(path, fileName, this);

            BindingContext = _viewModel;
            SettingsLayout.BindingContext = _viewModel.GetSettingsModel;
        }

        private void HideOrShowAsync_OnClicked(object sender, EventArgs e)
        {
            if (_animation == null) {
                _animation = new FrameAnimation(0, (uint)SettingsLayout.Height, 0);
            }
            _animation.RunShowOrHideAnimation(null, SettingsLayout, this, (int)SettingsLayout.Height == 0);
        }
    }
}