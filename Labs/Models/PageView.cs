using System;
using Xamarin.Forms;

namespace Labs.Models
{
    public static class PageView
    {
        public static Frame GetEntryTypeFrame(string placeHolder = "Answer")
        {
            var entry = new Entry
            {
                Placeholder = placeHolder,
                HorizontalOptions = LayoutOptions.Center
            };
            var frame = new Frame
            {
                Content = entry,
                Padding = 5,
                BorderColor = Color.FromHex("#CFD8DC"),
            };
            entry.TextChanged += (sender, e) =>
            {
                frame.BorderColor = e.NewTextValue != "" ? Color.FromHex("#03A9F4") : Color.FromHex("#E0E0E0");
            };
            return frame;
        }

        public static StackLayout GetResultPage(int time, int answersNum, int allAnswersNum, int mark)
        {
            var stack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            stack.Children.Add(new Label
            {
                Text = "Results",
                HorizontalOptions = LayoutOptions.Center,
                FontSize = 20
            });
            stack.Children.Add(new Label
            {
                Text = mark.ToString(),
                HorizontalOptions = LayoutOptions.Center
            });
            stack.Children.Add(new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    new Label
                    {
                        Text = "Time:",
                        HorizontalOptions = LayoutOptions.Fill
                    },
                    new Label{Text = time.ToString()}
                }
            });
            stack.Children.Add(new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    new Label
                    {
                        Text = "Right answers:",
                        HorizontalOptions = LayoutOptions.Fill
                    },
                    new Label{Text = $"{answersNum}/{allAnswersNum}"}
                }
            });

            return stack;
        }

        public static Label GetQuestionLabel(string question)
        {
            return new Label
            {
                Text = question,
                HorizontalOptions = LayoutOptions.Center,
                FontSize = 23,
                Margin = 12
            }; ;
        }
    }
}
