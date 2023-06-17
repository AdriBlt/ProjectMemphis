using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OneFlow.Clients;

namespace OneFlow.Tests
{
    [TestClass]
    public class ClientTest
    {
        [TestMethod]
        public async Task SearchTest()
        {
            // Given
            var client = new OneFlowClient();
            var query = "music";

            // When
            var search = await client.SearchGraphs(query).ConfigureAwait(false);

            // Then
            Assert.IsTrue(search.Graphs.Any());
        }

        [TestMethod]
        public async Task FetchTest()
        {
            // Given
            var client = new OneFlowClient();
            var guid = Guid.Parse("0514d4c6-bf2f-4539-838e-fa6f7bd13048");

            // When
            var graph = await client.FetchGraph(guid).ConfigureAwait(false);

            // Then
            Assert.AreEqual(guid, graph.GraphId);
        }
    }
}
