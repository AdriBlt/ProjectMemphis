using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using ProjectMemphis.Data;
using ProjectMemphis.Bot.Data;
using System.Linq;
using ProjectMemphis.Bot.Clients;

namespace ProjectMemphis.Bot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            switch (activity.Type)
            {
                case ActivityTypes.Message:
                    // Message received
                    await HandleMessageActivity(activity).ConfigureAwait(false);
                    break;
                case ActivityTypes.DeleteUserData:
                    // Implement user deletion here
                    // If we handle user deletion, return a real message
                    await ResetUserData(activity).ConfigureAwait(false);
                    break;
                case ActivityTypes.ConversationUpdate:
                    // Handle conversation state changes, like members being added and removed
                    // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                    // Not available in all channels
                    await ResetUserData(activity).ConfigureAwait(false);
                    await HandleMessageActivity(activity).ConfigureAwait(false);
                    break;
                case ActivityTypes.ContactRelationUpdate:
                    // Handle add/remove from contact lists
                    // Activity.From + Activity.Action represent what happened
                    break;
                case ActivityTypes.Typing:
                    // Handle knowing tha the user is typing
                    break;
                case ActivityTypes.Ping:
                    break;
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        private async Task ResetUserData(Activity activity)
        {
            var stateClient = activity.GetStateClient();
            var userData = await stateClient.BotState
                .GetUserDataAsync(activity.ChannelId, activity.From.Id)
                .ConfigureAwait(false);
            userData.RemoveProperty("UserData");
            await stateClient.BotState
                .SetUserDataAsync(activity.ChannelId, activity.From.Id, userData)
                .ConfigureAwait(false);
        }

        private async Task HandleMessageActivity(Activity activity)
        {
            // Retreive user data
            var stateClient = activity.GetStateClient();
            var userData = await stateClient.BotState.GetUserDataAsync(activity.ChannelId, activity.From.Id);
            var data = userData.GetProperty<UserData>("UserData");

            Node nextNode;

            if (data == default(UserData))
            {
                // Begining
                data = new UserData { CurrentStep = 0 };
                nextNode = (await ClientFactory.StoryStorageClient.GetDefaultStory().ConfigureAwait(false)).RootNode;
            }
            else
            {
                // Received Transition
                var transition = DeserializeTransition(activity.Text);

                if (transition == null)
                {
                    // null => user sent custom text
                    nextNode = null;
                }
                else if (transition.Step != data.CurrentStep
                         || transition.SourceNode.BotGuid != data.CurrentNode.BotGuid
                         || transition.SourceNode.NodeGuid != data.CurrentNode.NodeGuid)
                {
                    // different (guid || step) => escape the state
                    return;
                }
                else
                {
                    nextNode = transition.TargetNode;
                }
            }

            var previousNode = data.CurrentNode;

            var storyId = nextNode?.BotGuid ?? previousNode?.BotGuid;

            if (!storyId.HasValue)
            {
                // User data found but previous node null
                // Should not happend
                return;
            }

            var story = await ClientFactory.StoryStorageClient
                .GetStory(storyId.Value)
                .ConfigureAwait(false);

            // TODO HANDLE NULL STORY

            if (nextNode == null)
            {
                var previousState = story.GetState(previousNode.NodeGuid);
                if (previousState != null)
                {
                    if (!previousState.UserInputExpected && previousState.Events != null)
                    {
                        // Check if the input corresponds to one of the previous options
                        var targetNodesFromText = previousState.Events
                            .Where(e => e is OptionsEvent)
                            .SelectMany(e => ((OptionsEvent)e).Options)
                            .Where(o => o is StateOption && string.Compare(o.Text, activity.Text, true) == 0)
                            .Select(o => ((StateOption)o).TargetNode)
                            .ToList();
                        if (targetNodesFromText.Count == 1)
                        {
                            nextNode = targetNodesFromText.First();
                        }
                    }

                    if (nextNode == null && story.GetFallbackState != null)
                    {
                        nextNode = await story.GetFallbackState(previousState, activity.Text);

                        if (nextNode != null && nextNode.BotGuid != story.RootNode.BotGuid)
                        {
                            story = await ClientFactory.StoryStorageClient
                                .GetStory(nextNode.BotGuid)
                                .ConfigureAwait(false);
                            // TODO HANDLE NULL STORY
                        }
                    }
                }

                if (nextNode == null)
                {
                    nextNode = previousNode;
                }
            }

            if (nextNode.NodeGuid == default(Guid))
            {
                nextNode.NodeGuid = story.RootNode.NodeGuid;
            }

            // Update user data
            data.CurrentStep += 1;
            data.CurrentNode = nextNode;
            userData.SetProperty<UserData>("UserData", data);
            await stateClient.BotState.SetUserDataAsync(activity.ChannelId, activity.From.Id, userData).ConfigureAwait(false);

            if (previousNode == null || nextNode.BotGuid != previousNode.BotGuid)
            {
                // TODO Display separation / new tab
            }

            try
            {
                var nextState = story.GetState(nextNode.NodeGuid);
                var nextActivity = CreateNextActivity(activity, nextState, data.CurrentStep);
                var connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                await connector.Conversations.ReplyToActivityAsync(nextActivity);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private Activity CreateNextActivity(Activity previousActivity, State nextState, ulong nextStep)
        {
            if (previousActivity == null)
            {
                throw new ArgumentNullException(nameof(previousActivity));
            }

            if (nextState == null)
            {
                throw new ArgumentNullException(nameof(nextState));
            }

            var nextActivity = previousActivity.CreateReply();
            nextActivity.Text = nextState.Message;

            if (nextState.Events != null && nextState.Events.Any())
            {
                // TODO Handle TimerEvent differently
                nextActivity.Attachments = nextState.Events
                    .Select(e => e.ToAttachment(nextState.Node, nextStep))
                    .ToList();
            }

            return nextActivity;
        }

        private static Transition DeserializeTransition(string textValue)
        {
            try
            {
                return JsonConvert.DeserializeObject<Transition>(textValue);
            }
            catch (Exception)
            {
                return default(Transition);
            }
        }
    }
}