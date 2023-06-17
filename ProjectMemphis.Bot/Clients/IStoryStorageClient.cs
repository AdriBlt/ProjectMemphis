using System;
using System.Threading.Tasks;
using ProjectMemphis.Data;

namespace ProjectMemphis.Bot.Clients
{
    public interface IStoryStorageClient
    {
        Task<Story> GetDefaultStory();

        Task<Story> GetStory(Guid storyId);
    }
}