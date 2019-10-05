using System;
using System.Collections.Generic;
using System.Text;

namespace Labs.Interfaces
{
    public interface IFrameElement
    {
        string Id { get; set; }
        string MainText { get; set; }
        string Text { get; set; }
        bool IsRight { get; set; }
    }
}
