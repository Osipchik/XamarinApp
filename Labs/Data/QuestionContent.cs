using Labs.Models;
using Realms;

namespace Labs.Data
{
    public class QuestionContent : RealmObject
    {
        [PrimaryKey]
        public string Id { get; set; }
        public Question Owner { get; set; }

        public string MainText { get; set; }
        public string Text { get; set; }
        public bool IsRight { get; set; }
    }
}
