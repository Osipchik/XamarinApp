using System;

namespace Labs.Interfaces
{
    public interface ISettings
    {
        string Name { get; set; }
        string Subject { get; set; }
        string Question { get; set; }
        string Price { get; set; }
        string TotalCount { get; set; }
        string TotalPrice { get; set; }
        TimeSpan TimeSpan { get; set; }
    }
}