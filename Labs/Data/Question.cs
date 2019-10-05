using System;
using System.Collections.Generic;
using Realms;

namespace Labs.Data
{
    public class Question : RealmObject
    {
        [PrimaryKey]
        public string Id { get; set; }
        public TestModel Owner { get; set; }
        public int Type { get; set; }

        public string QuestionText { get; set; }
        public string Price { get; set; }
        public string Time { get; set; }

        public DateTimeOffset Date { get; set; }

        public IList<QuestionContent> Contents { get; }
    }
}
