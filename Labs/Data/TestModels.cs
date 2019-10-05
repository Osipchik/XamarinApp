using System;
using System.Collections.Generic;
using Labs.Models;
using Realms;

namespace Labs.Data
{
    public class TestModel : RealmObject
    {
        [PrimaryKey]
        public string Id { get; set; }
        public bool IsTemp { get; set; }

        public string Name { get; set; }
        public string Subject { get; set; }
        public DateTimeOffset Date { get; set; }
        public string Time { get; set; }
        public int TotalPrice { get; set; }

        public IList<Question> Questions { get; } 
    }
}
