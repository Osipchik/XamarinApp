using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using Labs.Models;
using Xamarin.Forms;

namespace Labs.Views
{
    public class EntryTypePageView
    {
        private int _coast;
        public int Index { get; }
        private string _answer = string.Empty;

        public EntryTypePageView(int coast, int index)
        {
            _coast = coast;
            Index = index;
        }

        public static StackLayout GetEntryTypeLayout(string path, int index, ref List<EntryTypePageView> entryTypePages)
        {
            var stack = new StackLayout();

            using (var reader = new StreamReader(path))
            {
                var frame = PageView.GetEntryTypeFrame();

                var entryTypePage = new EntryTypePageView(int.Parse(reader.ReadLine() ?? throw new InvalidOperationException()), index)
                {
                    _answer = reader.ReadLine()
                };

                stack.Children.Add(PageView.GetQuestionLabel(reader.ReadLine()));
                stack.Margin = 5;

                stack.Children.Add(frame);

                entryTypePages.Add(entryTypePage);
            }

            return stack;
        }

        public void CheckIt(ref ObservableCollection<View> source, ref int coast, ref int rightCount)
        {
            var stack = (StackLayout)(((ScrollView)source[Index]).Content);
            var frame = ((Frame)stack.Children[1]);
            var entry = (Entry)frame.Children[0];
            entry.IsReadOnly = true;

            var isRight = 1;
            if (entry.Text == _answer)
            {
                frame.BorderColor = Color.FromHex("#4CAF50");
            }
            else
            {
                frame.BorderColor = Color.FromHex("#f44336");
                _coast = isRight = 0;
            }

            coast = _coast;
            rightCount += isRight;
        }

        public void Disable(ref ObservableCollection<View> source)
        {
            var stack = (StackLayout)(((ScrollView)source[Index]).Content);
            var frame = ((Frame)stack.Children[1]);
            var entry = (Entry)frame.Children[0];
            entry.IsReadOnly = true;
        }
    }
}
