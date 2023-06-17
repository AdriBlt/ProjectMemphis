using System;
using System.Threading.Tasks;
using OneFlow.Data;

namespace OneFlow.Clients
{
    public interface IOneFlowClient
    {
        Task<OneFlowSearchResult> SearchGraphs(string query);

        Task<OneFlowFetchResult> FetchGraph(Guid id);
    }
}