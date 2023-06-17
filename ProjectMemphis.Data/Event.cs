using System;
using System.Collections.Generic;

namespace ProjectMemphis.Data
{
    // [JsonConverter(typeof(EventConverter))]
    public abstract class Event
    {
    }

    public class TimerEvent : Event
    {

    }

    public class OptionsEvent : Event
    {
        public string Title { get; set; }

        public IList<Option> Options { get; set; }
    }

    public class PictureEvent : Event
    {
        public string ImageUrl { get; set; }

        public string Alt { get; set; }

        public Node TargetNode { get; set; }
    }
}