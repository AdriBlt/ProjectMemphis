using System;

namespace ProjectMemphis.Data
{
    public class Transition
    {
        public Node SourceNode { get; set; }

        public Node TargetNode { get; set; }

        public ulong Step { get; set; }
    }
}