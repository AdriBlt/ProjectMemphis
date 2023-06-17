using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using OneFlow.Data;

namespace OneFlow.Tests
{
    [TestClass]
    public class OneFlowTest
    {
        [TestMethod]
        public void Load()
        {
            var oneFlowGraph = JsonConvert.DeserializeObject<OneFlowGraph>(File.ReadAllText(@"../../../oneflow.json"));
            Assert.AreEqual(16, oneFlowGraph.Steps.Count());
        }
    }
}
