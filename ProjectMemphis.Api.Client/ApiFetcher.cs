using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectMemphis.Data;

namespace ProjectMemphis.ApiClient
{
    internal class ApiFetcher : IApiClient
    {
        public Task<Story> GetStory(Guid guid)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Guid>> ListStoriesGuid()
        {
            throw new NotImplementedException();
        }

        public Task AddStory(Story story)
        {
            throw new NotImplementedException();
        }

        public Task UpdateStory(Story story)
        {
            throw new NotImplementedException();
        }

        public Task DeleteStory(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
