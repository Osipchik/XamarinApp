using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Labs.Helpers;
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
        private string _price;
        private string _time;
        private string _question;

        public CheckTypeViewModel(string path, string fileName)
        {
            Modificator = 0;
            FrameViewModel = new FrameViewModel();
            GetPath = path;
            _fileName = fileName;
            FileExist();
        }

        private void FileExist()
        {
            if (!string.IsNullOrEmpty(_fileName)){
                ReadFile(GetPath, _fileName);
            }
            else {
                FrameViewModel.AddNewModelAsync();
            }
        }

        public string GetPath { get; }

        public void RunHideOrShowAnimation(View view, IAnimatable owner, bool show)
        {
            if (_animation == null) {
                _animation = new FrameAnimation(0, (uint)view.Height, 0);
            }
            _animation.RunShowOrHideAnimation(null, view, owner, show);
        }

        private int _modificator;
        public int Modificator
        {
            get { return _modificator;}
            set
            {
                _modificator = value;
                if (_modificator != 0) {
                    FrameViewModel.DisableLastItem();
                }
            }
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
        public async Task<string> PageIsValidAsync(string question, string price, TimeSpan timeSpan,string seconds)
        {
            var message = await Task.Run(()=>GetMessage(question, price, seconds));
            if (string.IsNullOrEmpty(message)) {
                _stringsToSave = await GetStringsToSave(question, price, timeSpan, seconds);
            }

            return message;
        }
        private string GetMessage(string question, string price, string seconds)
        {
            var message = PageHelper.CheckQuestionPageSettings(question, price, seconds, FrameViewModel.Models.Count);
            message += CheckFrames();
            return message;
        }
        private string CheckFrames()
        {
            var message = string.Empty;
            if (FrameViewModel.Models.Any(model => string.IsNullOrEmpty(model.ItemTextLeft))) {
                message += AppResources.WarningAnswer;
            }

            return message;
        }
        

        private async Task<List<string>> GetPageSettingsAsync(string question, string price, TimeSpan timeSpan, string seconds)
        {
            return await Task.Run(() => new List<string> { PageHelper.NormalizeTime(timeSpan, seconds), price, question });
        }
        private List<string> GetFramesInfo()
        {
            var answers = string.Empty;
            var textList = new List<string>();
            foreach (var model in FrameViewModel.Models) {
                answers += model.isRight ? "0" : "1";
                textList.Add(model.ItemTextLeft);
            }

            var list = new List<string>{answers};
            list.AddRange(textList);

            return list;
        }
        private async Task<List<string>> GetStringsToSave(string question, string price, TimeSpan timeSpan, string seconds)
        {
            var stringsToSave = new List<string>();
            stringsToSave.AddRange(await GetPageSettingsAsync(question, price, timeSpan, seconds));
            stringsToSave.AddRange(await Task.Run(GetFramesInfo));

            return stringsToSave;
        }
        public void Save()
        {
            DirectoryHelper.SaveLinesToFile(Constants.TestTypeCheck, GetPath, _fileName, _stringsToSave);
        }



        public void GetTime(out TimeSpan timeSpan, out string seconds)
        {
            if (string.IsNullOrEmpty(_time)) {
                timeSpan = TimeSpan.Zero;
                seconds = "00";
            }
            else {
                SplitUpTimeLine(out var _timeSpan, out var _seconds);
                timeSpan = _timeSpan;
                seconds = _seconds;
            }
        }
        private void SplitUpTimeLine(out TimeSpan timeSpan, out string seconds)
        {
            PageHelper.GetTime(_time, out var _timeSpan, out var _seconds);
            timeSpan = _timeSpan;
            seconds = _seconds;
        }

        public string GetPrice()
        {
            var price = string.Empty;
            price = string.IsNullOrEmpty(_price) ? "00" : _price;

            return price;
        }

        public string GetQuestion()
        {
            return string.IsNullOrEmpty(_question) ? "" : _question;
        }

        private void ReadFile(string path, string fileName)
        {
            var strings = DirectoryHelper.ReadFile(path, fileName);
            SetPageSettings(strings);
            FillFramesAsync(strings, strings[3], 4);
        }

        private void SetPageSettings(IReadOnlyList<string> strings)
        {
            _time = strings[0];
            _price = strings[1];
            _question = strings[2];
        }

        private async void FillFramesAsync(IReadOnlyList<string> strings, string answers, int startIndex)
        {
            await Task.Run(() => {
                for (int i = startIndex; i < strings.Count; i++) {
                    FrameViewModel.AddModelAsync(strings[i], answers[i - startIndex] == '0');
                }
            });
        }
    }
}
