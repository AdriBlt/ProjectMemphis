using OneFlow.Clients;

namespace ProjectMemphis.Bot.Clients
{
    public static class ClientFactory
    {
        static ClientFactory()
        {
            StoryStorageClient = new OneFlowStoryClient(new OneFlowClient());
        }

        public static IStoryStorageClient StoryStorageClient { get; }
    }
}