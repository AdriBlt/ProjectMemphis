using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OneFlow.Data;
using ProjectMemphis.Data;

namespace ProjectMemphis.Bot.Data
{
    public static class OneFlowExtensionMethods
    {
        public static Story ToStory(this OneFlowGraph graph, Func<State, string, Task<Node>> fallback = null)
        {
            if (graph == null)
            {
                throw new ArgumentNullException(nameof(graph));
            }

            var steps = graph.Steps.ToList();
            var ids = new Dictionary<string, Guid>();
            steps.ForEach(step => ids[step.Id] = Guid.NewGuid());
            var finalStateSuccessId = Guid.NewGuid();
            var finalStateFailId = Guid.NewGuid();

            if (!ids.ContainsKey(graph.RootStepId))
            {
                throw new ArgumentOutOfRangeException($"Root Step Id {graph.RootStepId} is not contained in the graph.");
            }

            var rootGuid = ids[graph.RootStepId];
            var story = new Story
            {
                RootNode = new Node
                {
                    BotGuid = graph.Id,
                    NodeGuid = rootGuid
                },
                Name = graph.Title,
                States = new Dictionary<Guid, State>(),
                GetFallbackState = fallback
            };
            
            story.States[finalStateSuccessId] = new State
            {
                Node = new Node
                {
                    BotGuid = graph.Id,
                    NodeGuid = finalStateSuccessId
                },
                Message = "Your problem should now be solved, congratulations! This flow is now over, but you can go back in it if you need to find more information."
            };
            story.States[finalStateFailId] = new State
            {
                Node = new Node
                {
                    BotGuid = graph.Id,
                    NodeGuid = finalStateFailId
                },
                Message = "This flow is now over, sorry. :( If your problem is not solved yet, you can go back in the flow to find more information."
            };

            steps.ForEach(step =>
            {
                var id = ids[step.Id];
                var options = new List<Option>();

                new List<Tuple<OneFlowAnswer, string>>
                {
                    new Tuple<OneFlowAnswer, string>(step.Yes, "YES"),
                    new Tuple<OneFlowAnswer, string>(step.No, "NO"),
                    new Tuple<OneFlowAnswer, string>(step.Idk, "I DON'T KNOW"),
                    new Tuple<OneFlowAnswer, string>(step.Ok, "OK")
                }.ForEach(answer =>
                {
                    if (answer.Item1?.AnswerType == null)
                    {
                        return;
                    }

                    var option = default(Option);
                    switch (answer.Item1.AnswerType)
                    {
                        case OneFlowAnswerType.StepAnswer:
                            var targetId = answer.Item1.StepId;
                            if (!ids.ContainsKey(targetId))
                            {
                                throw new ArgumentOutOfRangeException($"Target Step Id {targetId} is not contained in the graph.");
                            }

                            option = new StateOption
                            {
                                TargetNode = new Node { BotGuid = graph.Id, NodeGuid = ids[targetId] }
                            };
                            break;
                        case OneFlowAnswerType.FinalAnswer:
                            option = new StateOption
                            {
                                TargetNode = new Node { BotGuid = graph.Id, NodeGuid = answer.Item1.IsSuccess ? finalStateSuccessId : finalStateFailId }
                            };
                            break;
                        case OneFlowAnswerType.UrlAnswer:
                            option = new UrlOption
                            {
                                Url = answer.Item1.Url
                            };
                            break;
                        case OneFlowAnswerType.GraphAnswer:
                            option = new StateOption
                            {
                                TargetNode = new Node { BotGuid = answer.Item1.GraphId }
                            };
                            break;
                        default:
                            throw new NotImplementedException(answer.Item1.AnswerType.ToString());
                    }

                    option.Text = answer.Item2;
                    options.Add(option);
                });

                story.States[id] = new State
                {
                    Node = new Node
                    {
                        BotGuid = graph.Id,
                        NodeGuid = id
                    },
                    Message = step.Text,
                    Events = new List<Event>
                    {
                        new OptionsEvent
                        {
                            Title = step.Question,
                            Options = options
                        }
                    }
                };
            });

            return story;
        }

        public static Option ToOption(this OneFlowResult result)
        {
            return new StateOption
            {
                Text = $"**{result.Title}** {result.Description}",
                TargetNode = new Node { BotGuid = result.Id }
            };
        }
    }
}