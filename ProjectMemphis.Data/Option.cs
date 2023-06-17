using System;

namespace ProjectMemphis.Data
{
    public abstract class Option
    {
        public string Text { get; set; }
    }

    public class StateOption : Option
    {
        public Node TargetNode { get; set; }
    }

    public class UrlOption : Option
    {
        public string Url { get; set; }
    }
}