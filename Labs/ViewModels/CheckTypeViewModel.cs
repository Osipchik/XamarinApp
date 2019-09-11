using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Labs.Helpers;
using Labs.Models;
using Labs.Resources;
using Xamarin.Forms;

namespace Labs.ViewModels
{
    public class CheckTypeViewModel 
    {
        private FrameAnimation _animation;
        private readonly string _fileName;
        private List<string> _stringsToSave;
        public readonly FrameViewModel FrameViewModel;
        private readonly PageSettingsViewModel _settingsViewModel;

        private int _modificator;
        public int Modificator
        {
            get => _modificator;
            set
            {
                _modificator = value;
                if (_modificator != 0)
                {
                    FrameViewModel.DisableLastItem();
                }
            }
        }
        public string GetPath { get; }

        public CheckTypeViewModel(string path, string fileName)
        {
            GetPath = path;
            _fileName = fileName;
            Modificator = 0;
            FrameViewModel = new FrameViewModel();
            _settingsViewModel = new PageSettingsViewModel();
            FileExist();
        }

        private void FileExist()
        {
            if (!string.IsNullOrEmpty(_fileName)){
                ReadFileAsync(GetPath, _fileName);
            }
            else {
                FrameViewModel.AddNewModelAsync();
            }
        }

        public PageSettingsModel GetSettingsModel => _settingsViewModel.SettingsModel;

        public void RunHideOrShowAnimation(View view, IAnimatable owner, bool show)
        {
            if (_animation == null) {
                _animation = new FrameAnimation(0, (uint)view.Height, 0);
            }
            _animation.RunShowOrHideAnimation(null, view, owner, show);
        }


        public async void TapEvent(int index)
        {
            switch (Modificator)
            {
                case -1:
                    await Task.Run(() => { FrameViewModel.ItemToDelete(index); });
                    break;
                case 0: goto default;
                case 1:
                    await Task.Run(() => { FrameViewModel.RightItems(index); });
                    break;
                default:
                    await Task.Run(() => { FrameViewModel.ItemIsWriteAble(index); });
                    break;
            }
        }


        public void GetAdditionalButtons(ImageButton crossButton, ImageButton acceptButton, int modificator)
        {
            if (crossButton.Source == null) crossButton.Source = "CrossBlack.png";
            if (modificator < 0) {
                SetCheckedRed(acceptButton);
            }
            else {
                SetCheckedBlack(acceptButton);
            }
        }
        private void SetCheckedRed(ImageButton agreeButton)
        {
            agreeButton.Opacity = 1;
            agreeButton.Source = "CheckedRed.png";
        }
        private void SetCheckedBlack(ImageButton agreeButton)
        {
            agreeButton.Opacity = 0.3;
            agreeButton.Source = "CheckedBlack.png";
        }


        // TODO: add to resources
        public async Task<string> PageIsValidAsync()
        {
            var message = await Task.Run(GetMessage);
            if (string.IsNullOrEmpty(message)) {
                _stringsToSave = await GetStringsToSave();
            }

            return message;
        }
        private string GetMessage()
        {
            var message = _settingsViewModel.CheckQuestionPageSettings();
            message += CheckFrames();
            return message;
        }
        private string CheckFrames()
        {
            var message = string.Empty;
            message += FrameViewModel.Models.Count < 1 ? "Add frame" : "";
            if (FrameViewModel.Models.Any(model => string.IsNullOrEmpty(model.ItemTextLeft))) {
                message += AppResources.WarningAnswer;
            }

            return message;
        }

        private List<string> GetFramesInfo()
        {
            var answers = string.Empty;
            var textList = new List<string>();
            foreach (var model in FrameViewModel.Models) {
                answers += model.IsRight ? "0" : "1";
                textList.Add(model.ItemTextLeft);
            }

            var list = new List<string>{answers};
            list.AddRange(textList);

            return list;
        }
        private async Task<List<string>> GetStringsToSave()
        {
            var stringsToSave = new List<string>();
            stringsToSave.AddRange(await _settingsViewModel.GetPageSettingsAsync());
            stringsToSave.AddRange(await Task.Run(GetFramesInfo));

            return stringsToSave;
        }
        public void Save()
        {
            DirectoryHelper.SaveFile(Constants.TestTypeCheck, GetPath, _fileName, _stringsToSave);
        }


        private async void ReadFileAsync(string path, string fileName)
        {
            var strings = DirectoryHelper.ReadStringsFromFile(path, fileName);
            await Task.Run(() => {
                _settingsViewModel.SetPageSettingsModel(strings[0], strings[1], strings[2]);
                FillFramesAsync(strings, strings[3], 4);
            });
        }
        private async void FillFramesAsync(IReadOnlyList<string> strings, string answers, int startIndex)
        {
            await Task.Run(() => {
                for (int i = startIndex; i < strings.Count; i++) {
                    FrameViewModel.AddModel(strings[i], answers[i - startIndex] == '0');
                }
            });
        }
    }
}
