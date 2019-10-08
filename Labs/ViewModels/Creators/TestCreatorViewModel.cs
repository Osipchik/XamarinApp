using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Labs.Data;
using Labs.Models;
using Labs.Resources;
using Labs.Views.Popups;
using Realms;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace Labs.ViewModels.Creators
{
    public class TestCreatorViewModel
    {
        private readonly string _testId;
        private readonly string _questionId;
        private readonly Repository.Type _questionType;
        private readonly SettingsViewModel _settingsViewModel;
        public readonly FrameViewModel FrameViewModel;

        public TestCreatorViewModel(Repository.Type type, string questionId, string testId = null)
        {
            _testId = testId;
            _questionId = questionId;
            _questionType = type;
            _settingsViewModel = new SettingsViewModel();
            FrameViewModel = new FrameViewModel();
            if (string.IsNullOrEmpty(questionId)) {
                InitializeNew();
            }
            else {
                InitializeExisting(questionId);
            }
        }

        public Grid GridButtons { get; set; }
        public Page Page { get; set; }
        public SettingsModel GetSettingsModel => _settingsViewModel.SettingsModel;
        public IEnumerable<FrameModel> GetFrameModels => FrameViewModel.Models;

        public ICommand AddItemCommand => FrameViewModel.AddItemCommand;
        public ICommand SaveFileCommand => new Command(SaveAsync);
        public ICommand DeleteCurrentFileCommand => new Command(DeleteQuestion);
        public ICommand HideGridButtonsCommand => new Command(HideGridButtons);
        public ICommand AcceptGridButtonCommand => new Command(AcceptGridButton);

        private void InitializeNew()
        {
            _settingsViewModel.SetEmptyModel();
            FrameViewModel.AddEmptyModel();
        }

        private async void InitializeExisting(string questionId)
        {
            await Task.Run(() =>
            {
                using (var realm = Realm.GetInstance())
                {
                    var question = realm.Find<Question>(questionId);
                    FrameViewModel.FillCreatorFrames(question.Contents, _questionType == Repository.Type.Check);
                    _settingsViewModel.SetSettingsModel(question.QuestionText, question.Price, question.Time);
                }
            });
        }

        private void HideGridButtons()
        {
            FrameViewModel.Modificator = 0;
            FrameViewModel.DisableAllAsync();
            GridButtons.IsVisible = false;
        }

        private async void AcceptGridButton()
        {
            if (FrameViewModel.Modificator < 0)
            {
                await Task.Run(() => FrameViewModel.DeleteItems());
            }
            HideGridButtons();
        }

        private async void SaveAsync()
        {
            await Task.Run(() => {
                if (PageIsValid().Result) {
                    using (var realm = Realm.GetInstance())
                    {
                        var question = realm.Find<Question>(_questionId);
                        var owner = realm.Find<TestModel>(_testId);
                        question = Repository.SetQuestionContent(realm, owner, question, _questionType,
                            _settingsViewModel.SettingsModel);
                        SaveContent(realm, question, Page.Navigation, FrameViewModel);
                    }
                }
            });
        }

        private void SaveContent(Realm realm, Question question, INavigation navigation, FrameViewModel viewModel)
        {
            SaveFrameContent(realm, question, viewModel.Models);
            Repository.RemoveQuestionContent(realm, question, viewModel.GetContentsToDelete);
            Device.BeginInvokeOnMainThread(async () => { await navigation.PopAsync(true); });
        }

        private void SaveFrameContent(Realm realm, Question question, IEnumerable<FrameModel> model)
        {
            foreach (var item in model) {
                if (item.Content != null) {
                    Repository.ContentUpdate(realm, question, item.Id, item);
                }
                else if (!string.IsNullOrEmpty(item.Id)) {
                    Repository.ContentCreate(realm, question, item);
                }
            }
        }

        private async void DeleteQuestion()
        {
            await Repository.RemoveQuestion(_testId, _questionId);
            await Page.Navigation.PopAsync(true);
        }

        private async Task<bool> PageIsValid()
        {
            var message = await Task.Run(GetMessage);
            var returnValue = string.IsNullOrEmpty(message);
            if (!returnValue) {
                Device.BeginInvokeOnMainThread(async () => {
                    await PopupNavigation.Instance.PushAsync(new WarningPopup(AppResources.Warning, message,
                        AppResources.Cancel));
                });
            }

            return returnValue;
        }

        private string GetMessage()
        {
            var message = _settingsViewModel.CheckPageSettings();
            message += CheckFrames();
            return message;
        }

        private string CheckFrames()
        {
            var message = string.Empty;
            message += FrameViewModel.Models.Count < 1 ? AppResources.AddAnswerOption : "";
            if (FrameViewModel.Models.Any(model => _settingsViewModel.CheckText(model.MainText))) {
                message += AppResources.WarningAnswer;
            }

            if (string.IsNullOrEmpty(message) && _questionType == Repository.Type.Stack) {
                if (FrameViewModel.Models.Any(model => _settingsViewModel.CheckText(model.Text))) {
                    message += AppResources.WarningAnswer;
                }
            }

            return message;
        }
    }
}
