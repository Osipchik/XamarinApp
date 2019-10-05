using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Labs.Helpers;
using Labs.Interfaces;
using Realms;

namespace Labs.Data
{
    public static class Repository
    {
        public enum Type
        {
            Check,
            Entry,
            Stack
        }

        public static TestModel GetTempTestModel(Realm realm) =>
            realm.All<TestModel>().Where(d => d.IsTemp).ToTestModel() ?? CreateTemp(realm);

        public static string GetTotalPrice(Realm realm, TestModel model)
        {
            var price = 0;
            realm.Write(() => { price += model.Questions.Sum(item => int.Parse(item.Price)); });
            return price.ToString();
        }

        private static TestModel CreateTemp(Realm realm)
        {
            var temp = new TestModel
            {
                Id = Guid.NewGuid().ToString(),
                IsTemp = true,
                Name = string.Empty,
                Subject = string.Empty,
                Date = DateTimeOffset.Now,
                Time = TimeSpan.Zero.TimeToString(),
                TotalPrice = 0
            };

            realm.Write(() => { realm.Add(temp); });
            
            return temp;
        }

        private static Question GetEmptyQuestion(Realm realm, Type type, TestModel owner)
        {
            var question = new Question
            {
                Id = Guid.NewGuid().ToString(),
                Owner = owner,
                Type = (int)type,
                Date = DateTimeOffset.Now,
                Time = DateTimeOffset.MinValue.ToString("T"),
                QuestionText = string.Empty,
                Price = "0"
            };

            realm.Write(() => { owner.Questions.Add(question); });
            return question;
        }

        public static Question SetQuestionContent(Realm realm, TestModel owner, Question question, Type type, ISettings settings)
        {
            if (question == null) {
                question = GetEmptyQuestion(realm, type, owner);
            }

            realm.Write(() =>
            {
                question.QuestionText = settings.Question;
                question.Price = settings.Price;
                question.Time = settings.TimeSpan.TimeToString();

                if (!owner.Questions.Contains(question))
                {
                    owner.Questions.Add(question);
                }
            });

            return question;
        }

        public static void ContentCreate(Realm realm, Question owner, IFrameElement item)
        {
            realm.Write(() =>
            {
                owner.Contents.Add(new QuestionContent
                {
                    Id = item.Id,
                    Owner = owner,
                    MainText = item.MainText,
                    Text = item.Text,
                    IsRight = item.IsRight,
                });
            });
        }
        
        public static void ContentUpdate(Realm realm, Question question, string id, IFrameElement item)
        {
            realm.Write(() =>
            {
                var questionContent = question.Contents.First(d => d.Id == id);
                questionContent.MainText = item.MainText;
                questionContent.Text = item.Text;
                questionContent.IsRight = item.IsRight;
            });
        }

        public static void RemoveQuestionContent(Realm realm, Question owner, IEnumerable<QuestionContent> contentList)
        {
            realm.Write(() =>
            {
                foreach (var content in contentList) {
                    owner.Contents.Remove(content);
                }
            });
        }

        public static async Task RemoveTestAsync(string testId)
        {
            await Task.Run(() => {
                using (var realm = Realm.GetInstance())
                {
                    realm.Write(() => { realm.Remove(realm.Find<TestModel>(testId)); });
                }
            });
        }

        public static async Task RemoveQuestion(string ownerId, string id)
        {
            await Task.Run(() => {
                if(string.IsNullOrEmpty(id)) return;
                using (var realm = Realm.GetInstance())
                {
                    var test = realm.Find<TestModel>(ownerId);
                    realm.Write(() =>
                    {
                        test.Questions.Remove(realm.Find<Question>(id));
                    });
                }
            });
        }
    }
}
