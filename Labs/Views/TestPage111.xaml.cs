using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CarouselView.FormsPlugin.Abstractions;
using Labs.Resources;
using Labs.ViewModels;
using Labs.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Labs
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TestPage111
    {
        //private readonly PageViewModel _pageViewModel;
        //private readonly List<TimerViewModel> _timerViewModelList;
        //private int _oldValue = 0;
        //private int _testTime = 0, _rightCount = 0, _price = 0;
        //private bool _isChecked = true;

        //public TestPage111(string path)
        //{
        //    InitializeComponent();

        //    _pageViewModel = new PageViewModel();
        //    _timerViewModelList = new List<TimerViewModel>();
        //    _pageViewModel.InitContentAsync(path);

        //    ReadSettings(path);
        //    TestTimerAsync();

        //    carousel.BindingContext = _pageViewModel;
        //}

        //private async void Handle_PositionSelected(object sender, PositionSelectedEventArgs e)
        //{
        //    if (_timerViewModelList.Count <= 1) return;

        //    await Task.Run(() =>
        //    {
        //        var index = Move(e.NewValue);
        //        carousel.Position = index;

        //        _timerViewModelList[_oldValue].TimerStop();
        //        _timerViewModelList[index].TimerRunAsync();

        //        progressBar.BindingContext = _timerViewModelList[index];
        //        labelTime.BindingContext = _timerViewModelList[index];
        //        _oldValue = index;
        //    });
        //}

        //private int Move(int newValue)
        //{
        //    var index = newValue;
        //    if (newValue > _oldValue){
        //        index = MoveRight(newValue);
        //        if (index == -1)
        //            index = MoveLeft(newValue);
        //    }

        //    if (newValue < _oldValue){
        //        index = MoveLeft(newValue);
        //        if (index == -1)
        //            index = MoveRight(newValue);
        //    }

        //    if (index == -1){
        //        ButtonCheck_OnClicked(this, EventArgs.Empty);
        //        return 0;
        //    }

        //    return index;
        //}

        //private int MoveRight(int index)
        //{
        //    while (index < _pageViewModel.ItemsSource.Count)
        //    {
        //        if (!_pageViewModel.IsDisable(index)) return index;
        //        index++;
        //    }

        //    return -1;
        //}

        //private int MoveLeft(int index)
        //{
        //    while (index >= 0)
        //    {
        //        if (!_pageViewModel.IsDisable(index)) return index;
        //        index--;
        //    }

        //    return -1;
        //}


        //private void ReadSettings(string path)
        //{
        //    using (var reader = new StreamReader(Path.Combine(path, "settings.txt")))
        //    {
        //        reader.ReadLine();
        //        reader.ReadLine();

        //        var forTest = reader.ReadLine();
        //        var type = reader.ReadLine();
        //        var time = double.Parse(reader.ReadLine());

        //        if (type == "-1" || time == 0)
        //        {
        //            absoluteLayout.IsVisible = false;
        //            return;
        //        }

        //        if (forTest == "True") SetTimerForTest(type, time);
        //        else SetTimerForQuestion(type, time);
        //    }
        //}

        //private void SetTimerForTest(string type, double time)
        //{
        //    var timer = new TimerViewModel(TimerOffForTestAsync);
        //    timer.SetTimer(type, time);
        //    _timerViewModelList.Add(timer);
        //    progressBar.BindingContext = _timerViewModelList[0];
        //    labelTime.BindingContext = _timerViewModelList[0];
        //}

        //private void SetTimerForQuestion(string type, double time)
        //{
        //    var count = _pageViewModel.ItemsSource.Count;

        //    var timerFirst = new TimerViewModel(TimerOffForQuestion);
        //    timerFirst.SetTimer(type, time);
        //    _timerViewModelList.Add(timerFirst);
        //    progressBar.BindingContext = _timerViewModelList[0];
        //    labelTime.BindingContext = _timerViewModelList[0];

        //    for (var i = 1; i < count; i++)
        //    {
        //        var timer = new TimerViewModel(TimerOffForQuestion);
        //        timer.SetTimer(type, time);
        //        timer.TimerStop();
        //        _timerViewModelList.Add(timer);
        //    }
        //}

        //private async void TimerOffForTestAsync()
        //{
        //    await Task.Run(() =>
        //    {
        //        foreach (var model in _timerViewModelList)
        //        {
        //            model.TimerStop();
        //        }

        //        _timerViewModelList.Clear();
        //    });
        //    await Task.Run(_pageViewModel.DisableAll);
        //}

        //private void TimerOffForQuestion()
        //{
        //    _pageViewModel.DisableAsync(_oldValue);
        //}

        //private async void TestTimerAsync()
        //{
        //    await Task.Run(() =>
        //    {
        //        Device.StartTimer(TimeSpan.FromSeconds(1), () =>
        //        {
        //            _testTime++;
        //            return _isChecked;
        //        });
        //    });
        //}

        //private async void ButtonCheck_OnClicked(object sender, EventArgs e)
        //{
        //    if (_isChecked)
        //    {
        //        TimerOffForTestAsync();
        //        _isChecked = false;
        //        _pageViewModel.CheckIt(ref _price, ref _rightCount);
        //    }

        //    await DisplayAlert(AppResources.Result, 
        //                       AppResources.TestSettingsTime + $" {_testTime}\n" +
        //                              AppResources.Mark + $" {_price}\n" +
        //                              AppResources.Rights + $" {_rightCount}/{_pageViewModel.ItemsSource.Count}", AppResources.Cancel);
        //}


        //protected override bool OnBackButtonPressed()
        //{
        //    Device.BeginInvokeOnMainThread(async () =>
        //    {
        //        var result = await DisplayAlert("", AppResources.Escape, AppResources.Yes, AppResources.No);
        //        if (result)
        //        {
        //            MessagingCenter.Send<Page>(this, "StartInfoUpLoad");
        //            Back();
        //        }
        //    });

        //    return true;
        //}

        //private async void Back()
        //{
        //    foreach (var model in _timerViewModelList)
        //    {
        //        model.TimerStop();
        //    }
        //    await Navigation.PopModalAsync(true);
        //}

        //private void Button_OnClicked(object sender, EventArgs e)
        //{
        //    OnBackButtonPressed();
        //}
    }
}