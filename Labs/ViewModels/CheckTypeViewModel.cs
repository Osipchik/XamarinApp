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
        private IEnumerable<string> _stringsToSave;
        public readonly FrameViewModel FrameViewModel;
        public CheckTypeViewModel(string path)
        {
            GetPath = path;
            Modificator = 0;
            FrameViewModel = new FrameViewModel();
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
                case 0:
                    await Task.Run(() => { FrameViewModel.ItemIsWriteAble(index); });
                    break;
                case 1:
                    await Task.Run(() => { FrameViewModel.RightItems(index); });
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
        private IEnumerable<string> GetFramesInfo()
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
        private async Task<IEnumerable<string>> GetStringsToSave(string question, string price, TimeSpan timeSpan, string seconds)
        {
            var stringsToSave = new List<string>();
            stringsToSave.AddRange(await GetPageSettingsAsync(question, price, timeSpan, seconds));
            stringsToSave.AddRange(await Task.Run(GetFramesInfo));

            return stringsToSave;
        }

        public void Save()
        {
            var asd = _stringsToSave;
        }
    }
}
