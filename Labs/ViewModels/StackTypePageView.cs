using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Labs.Models;
using Xamarin.Forms;

namespace Labs.ViewModels
{
    class StackTypePageView
    {
        public int Index { get; }
        private int _coast;
        private readonly List<string> _answers;
        private readonly List<string> _textLeftList;
        private List<string> _textRightList;

        private static Tap _tapCommand;

        public StackTypePageView(int index)
        {
            Index = index;
            _answers = new List<string>();
            _textLeftList = new List<string>();
            _textRightList = new List<string>();
            _tapCommand = new Tap();
        }

        public static StackLayout GetStackTypePage(string path, int index, ref List<StackTypePageView> stackTypePages)
        {
            var stack = new StackLayout();
            var stackTypePage = new StackTypePageView(index);

            ReadFile(path, ref stackTypePage, out var question);
            FillAnswer(ref stackTypePage);
            ShakeList(ref stackTypePage._textRightList);

            stack.Children.Add(PageView.GetQuestionLabel(question));

            for (int i = 0, total = stackTypePage._answers.Count; i < total; i++)
            {
                stack.Children.Add(GetFrame(stackTypePage._textLeftList[i], stackTypePage._textRightList[i]));
            }
            stackTypePages.Add(stackTypePage);

            return stack;
        }

        private static void ReadFile(string path, ref StackTypePageView stackTypePage, out string question)
        {
            using (var reader = new StreamReader(path))
            {
                stackTypePage._coast = int.Parse(reader.ReadLine());
                question = reader.ReadLine();
                question = reader.ReadLine();

                var textLeft = string.Empty;
                while ((textLeft = reader.ReadLine()) != null)
                {
                    stackTypePage._textLeftList.Add(textLeft);
                    stackTypePage._textRightList.Add(reader.ReadLine());
                }
            }
        }

        private static void FillAnswer(ref StackTypePageView stackTypePage)
        {
            for (int i = 0, total = stackTypePage._textLeftList.Count; i < total; i++)
            {
                stackTypePage._answers.Add(stackTypePage._textLeftList[i] + stackTypePage._textRightList[i]);
            }
        }

        private static void ShakeList(ref List<string> list)
        {
            var rand = new Random();
            int count = list.Count;
            for (int i = 0; i < count; i++)
            {
                var index = rand.Next(count);
                var buf = list[index];
                list[index] = list[i];
                list[i] = buf;
            }
        }

        private static Frame GetFrame(string textLeft, string textRight)
        {
            var grid = new Grid
            {
                RowDefinitions = { new RowDefinition { Height = GridLength.Auto } },
                ColumnDefinitions =
                {
                    new ColumnDefinition {Width = GridLength.Star},
                    new ColumnDefinition {Width = GridLength.Star}
                },
                ColumnSpacing = 0
            };
            grid.Children.Add(new Label
            {
                Text = textLeft,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                FontSize = 20,
                Margin = 3
            }, 0, 0);


            grid.Children.Add(new Label
            {
                Text = textRight,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                FontSize = 20,
                Margin = 3
            }, 1, 0);

            var frame = new Frame
            {
                Content = grid,
                Padding = 5,
                BorderColor = Color.FromHex("#CFD8DC")
            };

            var clickEvent = new TapGestureRecognizer();
            clickEvent.Tapped += (s, e) => { _tapCommand.TapCommand(frame); };
            frame.GestureRecognizers.Add(clickEvent);

            return frame;
        }

        public void CheckIt(ref ObservableCollection<View> source, ref int coast, ref int rightCount)
        {
            var stack = (StackLayout)((ScrollView)source[Index]).Content;

            var isRight = 1;
            for (int i = 1, total = stack.Children.Count; i < total; i++)
            {
                var frame = ((Frame)stack.Children[i]);
                var grid = (Grid)frame.Children[0];
                var textLeft = ((Label) grid.Children[0]).Text;
                var textRight = ((Label) grid.Children[1]).Text;
                frame.GestureRecognizers[0] = null;

                if ((textLeft + textRight) == _answers[i - 1])
                {
                    frame.BorderColor = Color.FromHex("#4CAF50");
                }
                else
                {
                    frame.BorderColor = Color.FromHex("#f44336");
                    _coast = isRight = 0;
                }
            }

            coast = _coast;
            rightCount += isRight;
        }

        public void Disable(ref ObservableCollection<View> source)
        {
            var stack = (StackLayout) ((ScrollView) source[Index]).Content;
            for (int i = 1, total = stack.Children.Count; i < total; i++)
            {
                var frame = ((Frame) stack.Children[i]);
                frame.GestureRecognizers[0] = null;
            }
        }
    }


    public class Tap
    {
        private Frame _frameOne;
        private Frame _frameTwo;

        public void TapCommand(Frame frame)
        {
            if (_frameOne == null)
            {
                _frameOne = frame;
                _frameOne.BorderColor = Color.FromHex("#03A9F4");
                ((Label)((Grid)_frameOne.Children[0]).Children[1]).TextColor = Color.FromHex("#03A9F4");
            }
            else
            {
                _frameTwo = frame;
                _frameTwo.BorderColor = Color.FromHex("#03A9F4");
                ((Label)((Grid)_frameTwo.Children[0]).Children[1]).TextColor = Color.FromHex("#03A9F4");
            }

            if (_frameOne != null && _frameTwo != null)
            {
                SwapLabel();
                _frameOne.BorderColor = _frameTwo.BorderColor = Color.FromHex("#E0E0E0");
                _frameOne = _frameTwo = null;
            }
        }

        private void SwapLabel()
        {
            var labelOne = (Label)((Grid)_frameOne.Children[0]).Children[1];
            var labelTwo = (Label)((Grid)_frameTwo.Children[0]).Children[1];

            labelOne.TextColor = labelTwo.TextColor = Color.FromHex("#757575");

            var buf = labelOne.Text;
            labelOne.Text = labelTwo.Text;
            labelTwo.Text = buf;
        }
    }
}
