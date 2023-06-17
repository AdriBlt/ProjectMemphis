using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ProjectMemphis.Data
{
    public class State
    {
        public Node Node { get; set; }

        public string Message { get; set; }

        [JsonProperty(TypeNameHandling = TypeNameHandling.Auto)]
        public IEnumerable<Event> Events { get; set; }

        public bool UserInputExpected { get; set; } = false;
    }
}