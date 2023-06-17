using System;
using System.Collections.Generic;

namespace OneFlow.Data
{
    public class OneFlowGraph
    {
        public string Namespace { get; set; }

        public Guid Id { get; set; }

        public string Title { get; set; }

        public IEnumerable<string> Tags { get; set; }

        public string Owner { get; set; }

        public string OwnerTeam { get; set; }
        
        public string RootStepId { get; set; }

        public IEnumerable<OneFlowNode> Steps { get; set; }
    }
}
