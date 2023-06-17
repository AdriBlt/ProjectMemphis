using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using OneFlow.Data;
using ProjectMemphis.Bot.Data;

namespace ProjectMemphis.Bot.Tests
{
    [TestClass]
    public class OneFlowTests
    {
        [TestMethod]
        public void Load()
        {
            const string json = @"C:\Source\Repos\Project Memphis\ProjectMemphis\oneflow.json";
            var oneFlowGraph = JsonConvert.DeserializeObject<OneFlowGraph>(File.ReadAllText(json));
            var story = oneFlowGraph.ToStory();
            Assert.AreEqual(oneFlowGraph.Steps.Count() + 2, story.States.Count); // Adding 2 final states
            Console.WriteLine(story.States.Count);
        }
    }
}
