using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectMemphis.Bot.Data;
using ProjectMemphis.Data;

namespace ProjectMemphis.Bot.Clients
{
    public class StoryStorageClient : IStoryStorageClient
    {
        private readonly IDictionary<Guid, Story> _stories;

        public StoryStorageClient()
        {
            _stories = new Dictionary<Guid, Story>();
            var story = MockStory.GetOneFlowStory();
            _stories[story.RootNode.BotGuid] = story;
        }

        public Task<Story> GetDefaultStory() => Task.FromResult(_stories.Values.FirstOrDefault());

        public Task<Story> GetStory(Guid storyId) => Task.FromResult(_stories[storyId]);
    }
}