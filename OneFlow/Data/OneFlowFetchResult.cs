using System;

namespace OneFlow.Data
{
    public class OneFlowFetchResult
    {
        public Guid GraphId { get; set; }

        public string GraphUrl { get; set; }

        public OneFlowGraph Graph { get; set; }
    }
}
