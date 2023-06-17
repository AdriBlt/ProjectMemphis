using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectMemphis.Data;

namespace ProjectMemphis.ApiClient
{
    public interface IApiClient
    {
        Task<IList<Guid>> ListStoriesGuid();
        Task<Story> GetStory(Guid guid);
        Task AddStory(Story story);
        Task UpdateStory(Story story);
        Task DeleteStory(Guid id);
    }
}