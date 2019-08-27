using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Labs.Models;
using Xamarin.Forms;

namespace Labs.Views
{
    public class CheckTypePageView
    {
        public int Index { get; }
        private int _coast;
        private readonly List<bool> _answers;

        public CheckTypePageView(int index, int coast)
        {
            Index = index;
            _coast = coast;
            _answers = new List<bool>();
        }

        public static StackLayout GetCheckTypeLayout(string path, int index, ref List<CheckTypePageView> checkTypePages)
        {
            var stack = new StackLayout();

            using (var reader = new StreamReader(path))
            {
                var coast = reader.ReadLine();
                var checkTypePage = new CheckTypePageView(index, int.Parse(coast));
                var isChecked = reader.ReadLine();

                stack.Children.Add(PageView.GetQuestionLabel(reader.ReadLine()));
                stack.Margin = 5;

                var count = 0;
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    stack.Children.Add(PageView.GetCheckTypeFrame(line));
                    checkTypePage._answers.Add(isChecked != null && isChecked[count] == '1');
                    count++;
                }

                checkTypePages.Add(checkTypePage);
            }

            return stack;
        }

        public void CheckIt(ref ObservableCollection<View> source, ref int coast, ref int rightCount)
        {
            var stack = (StackLayout)((ScrollView)source[Index]).Content;

            var isRight = 1;
            for (int i = 1, total = stack.Children.Count; i < total; i++)
            {
                var frame = ((Frame)stack.Children[i]);
                frame.GestureRecognizers[0] = null;

                var isChecked = frame.BorderColor == Color.FromHex("#03A9F4");

                if (_answers[i - 1] == isChecked)
                {
                    frame.BorderColor = Color.FromHex("#4CAF50");
                }
                else
                {
                    frame.BorderColor = Color.FromHex("#f44336");
                    _coast = isRight = 0;
                }
            }

            coast += _coast;
            rightCount += isRight;
        }

        public void Disable(ref ObservableCollection<View> source)
        {
            var stack = (StackLayout)((ScrollView)source[Index]).Content;
            for (int i = 1, total = stack.Children.Count; i < total; i++)
            {
                var frame = ((Frame)stack.Children[i]);
                frame.GestureRecognizers[0] = null;
            }
        }
    }
}
