using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectMemphis.Data
{
    public class Story
    {
        public string Name { get; set; }

        public string Author { get; set; }

        public Node RootNode { get; set; }

        public IDictionary<Guid, State> States { get; set; }

        public Func<State, string, Task<Node>> GetFallbackState { get; set; }

        public State GetState(Guid guid) => States[guid];
    }
}
