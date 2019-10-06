using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Labs.Annotations;
using Labs.Data;
using Labs.Helpers;
using Labs.Models;
using Labs.Resources;
using Labs.Views;
using Labs.Views.Creators;
using Labs.Views.Popups;
using Realms;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace Labs.ViewModels.Creators
{
    public sealed class MenuCreatorViewModel : INotifyPropertyChanged
    {
        private readonly FrameViewModel _frameViewModel;
        private readonly SettingsViewModel _settingsViewModel;
        public string TestId;

        public MenuCreatorViewModel(string testId = null)
        {
            _frameViewModel = new FrameViewModel();
            _settingsViewModel = new SettingsViewModel();
            TestId = testId;

            InitializeAsync(testId);
        }

        public Page Page { get; set; }
        public IEnumerable<FrameModel> GetFrameModels => _frameViewModel.Models;
        public SettingsModel GetSettings => _settingsViewModel.SettingsModel;

        public ICommand CreateCheckTypePageCommand => new Command(OpenCheckCreator());
        public ICommand CreateEntryTypePageCommand => new Command(OpenEntryCreator());
        public ICommand CreateStackTypePageCommand => new Command(OpenStackCreator());
        public ICommand SaveTestCommand => new Command(Save);
        public ICommand DeleteTestCommand => new Command(Delete);

        public async void InitializeAsync(string testId)
        {
            _frameViewModel.Models.Clear();
            await Task.Run(() => {
                using (var realm = Realm.GetInstance())
                {
                    var test = string.IsNullOrEmpty(testId) ? Repository.GetTempTestModel(realm) : realm.Find<TestModel>(testId);
                    FillQuestionFrames(test.Questions);
                    SetSettings(realm, test);
                    TestId = test.Id;
                }
            });
        }

        private void SetSettings(Realm realm, TestModel model)
        {
            if (model == null) {
                _settingsViewModel.SetEmptyModel();
            }
            else {
                _settingsViewModel.SetTestSettingsModel(realm, model);
            }
        }

        private void FillQuestionFrames(IEnumerable<Question> questions)
        {
            foreach (var item in questions) {
                _frameViewModel.Models.Add(new FrameModel 
                {
                    Id = item.Id,
                    QuestionType = item.Type,
                    MainText = item.QuestionText,
                    Text = item.Date.ToString("d"),
                    TextUnderMain = item.Price
                });
            }
        }

        private Action OpenCheckCreator(string questionId = null) =>
            async () => { await Page.Navigation.PushAsync(new TypeCheckCreatingPage(questionId, TestId)); };

        private Action OpenStackCreator(string questionId = null) =>
            async () => { await Page.Navigation.PushAsync(new TypeStackCreatingPage(questionId, TestId)); };

        private Action OpenEntryCreator(string questionId = null) =>
            async () => { await Page.Navigation.PushAsync(new TypeEntryCreatingPage(questionId, TestId)); };

        public void OpenCreatingPage(int index)
        {
            var question = _frameViewModel.Models[index];
            switch (question.QuestionType)
            {
                case (int)Repository.Type.Check:
                    OpenCheckCreator(question.Id).Invoke();
                    break;
                case (int)Repository.Type.Stack:
                    OpenStackCreator(question.Id).Invoke();
                    break;
                case (int)Repository.Type.Entry:
                    OpenEntryCreator(question.Id).Invoke();
                    break;
            }
        }

        private async void Save()
        {
            if (PageIsValid()) {
                await Task.Run(() => {
                    using (var realm = Realm.GetInstance())
                    {
                        var test = realm.Find<TestModel>(TestId);
                        test.Realm.Write(() =>
                        {
                            test.Name = _settingsViewModel.SettingsModel.Name;
                            test.Subject = _settingsViewModel.SettingsModel.Subject;
                            test.Time = _settingsViewModel.SettingsModel.TimeSpan.TimeToString();
                            test.IsTemp = false;
                        });
                    }
                    InitializeAsync(TestId);
                });
                await PushBackAsync();
            }
        }

        private async Task PushBackAsync()
        {
            if (Page.Navigation.NavigationStack.Count == 1) {
                await Page.Navigation.PushAsync(new HomePage());
                Page.Navigation.RemovePage(Page);
            }
            else {
                await Page.Navigation.PopAsync(true);
            }
        }

        private async void Delete()
        {
            await Repository.RemoveTestAsync(TestId);
            Page.Navigation.InsertPageBefore(new HomePage(), Page);
            await Page.Navigation.PopToRootAsync(true);
        }

        private bool PageIsValid()
        {
            var message = GetMessage();
            var returnValue = string.IsNullOrEmpty(message);
            if (!returnValue) {
                Device.BeginInvokeOnMainThread(async () =>
                    await PopupNavigation.Instance.PushAsync(new WarningPopup(AppResources.Warning, message,
                        AppResources.Cancel)));
            }

            return returnValue;
        }

        private string GetMessage()
        {
            var message = _settingsViewModel.CheckCreatorMenuPageSettings();
            message += _frameViewModel.Models.Count < 1 ? AppResources.AddTestPage : string.Empty;
            return message;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
