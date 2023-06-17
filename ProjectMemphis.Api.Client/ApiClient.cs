using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectMemphis.Data;

namespace ProjectMemphis.ApiClient
{
    public class ApiClient : IApiClient
    {
        private readonly ApiFetcher _fetcher;

        public ApiClient()
        {
            _fetcher = new ApiFetcher();
        }

        public Task<IList<Guid>> ListStoriesGuid() => _fetcher.ListStoriesGuid();

        public Task<Story> GetStory(Guid guid) => _fetcher.GetStory(guid);

        public Task AddStory(Story story) => _fetcher.AddStory(story);
        public Task UpdateStory(Story story) => _fetcher.UpdateStory(story);

        public Task DeleteStory(Guid id) => _fetcher.DeleteStory(id);
    }
}
