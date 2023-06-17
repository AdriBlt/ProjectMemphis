using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OneFlow.Clients;
using ProjectMemphis.Bot.Data;
using ProjectMemphis.Data;

namespace ProjectMemphis.Bot.Clients
{
    public class OneFlowStoryClient : IStoryStorageClient
    {
        private readonly IOneFlowClient _oneFlowClient;
        private readonly Story _searchStory;
        private Node _confirmationNode;
        private Node _noResultNode;
        private Node _resultDisplayNode;
        private StateOption _cancelStateOption;
        private OptionsEvent _resultOptions;
        private Dictionary<Guid, Story> _stories;

        public OneFlowStoryClient(IOneFlowClient oneFlowClient)
        {
            _oneFlowClient = oneFlowClient;
            _searchStory = CreateSearchStory();
            _stories = new Dictionary<Guid, Story>
            {
                [_searchStory.RootNode.BotGuid] = _searchStory
            };
        }

        private Story CreateSearchStory()
        {
            var rootNode = new Node
            {
                BotGuid = Guid.NewGuid(),
                NodeGuid = Guid.NewGuid()
            };
            _confirmationNode = new Node
            {
                BotGuid = rootNode.BotGuid,
                NodeGuid = Guid.NewGuid()
            };
            _noResultNode = new Node
            {
                BotGuid = rootNode.BotGuid,
                NodeGuid = Guid.NewGuid()
            };
            _resultDisplayNode = new Node
            {
                BotGuid = rootNode.BotGuid,
                NodeGuid = Guid.NewGuid()
            };
            _cancelStateOption = new StateOption { Text = "Cancel", TargetNode = rootNode };
            _resultOptions = new OptionsEvent
            {
                Title = "Click on a link or search again..."
            };

            var states = new List<State>(4)
            {
                // Search State
                new State
                {
                    Node = rootNode,
                    Message = "Welcome to Oneflow! Type your search query:",
                    UserInputExpected = true
                },
                // Confirmation state
                new State
                {
                    Node = _confirmationNode,
                    Message = "You are about to exit the current graph to search for a new graph.",
                    Events = new List<Event>
                    {
                        new OptionsEvent
                        {
                            Title = "Are you sure you want to proceed?",
                            Options = new List<Option>
                            {
                                _cancelStateOption,
                                new StateOption { Text = "OK", TargetNode = rootNode }
                            }
                        }
                    }
                },
                // No result state
                new State
                {
                    Node = _noResultNode,
                    Message = "No results were found. Please try again!",
                    UserInputExpected = true
                },
                // Result display state
                new State
                {
                    Node = _resultDisplayNode,
                    Message = "Here are the results for your search.",
                    Events = new List<Event>(1) { _resultOptions },
                    UserInputExpected = true
                }
            };

            return new Story
            {
                Author = "Microsoft MediaServices OneFlow",
                Name = "OneFlow Search",
                RootNode = rootNode,
                States = states.ToDictionary(state => state.Node.NodeGuid),
                GetFallbackState = (state, text) =>
                {
                    if (state.UserInputExpected)
                    {
                        return SearchForOneFlowGraph(text);
                    }

                    return Task.FromResult(rootNode);
                }
            };
        }

        private async Task<Node> SearchForOneFlowGraph(string text)
        {
            try
            {
                var results = await _oneFlowClient.SearchGraphs(text).ConfigureAwait(false);
                if (results != null && results.Graphs.Any())
                {
                    _resultOptions.Options = results.Graphs.Select(g => g.ToOption()).ToList();
                    return _resultDisplayNode;
                }
            }
            catch (Exception e)
            {
                // No connection
            }
            return _noResultNode;
        }

        public Task<Story> GetDefaultStory()
        {
            return Task.FromResult(_searchStory);
        }

        public async Task<Story> GetStory(Guid storyId)
        {
            Story story;
            if (_stories.TryGetValue(storyId, out story))
            {
                return story;
            }

            try
            {
                story = await _oneFlowClient.FetchGraph(storyId)
                    .ContinueWith(t => t.Result?.Graph?.ToStory(SearchNodeCallback))
                    .ConfigureAwait(false);
                if (story != null)
                {
                    _stories[storyId] = story;
                    return story;
                }
            }
            catch (Exception e)
            {
                // No connection
            }

            return _searchStory;
        }

        internal Task<Node> SearchNodeCallback(State currentState, string text)
        {
            _cancelStateOption.TargetNode = currentState.Node;
            return Task.FromResult(_confirmationNode);
        }
    }
}