using ProjectMemphis.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using OneFlow.Data;

namespace ProjectMemphis.Bot.Data
{
    public static class MockStory
    {
        public static Story GetOneFlowStory()
        {
            const string json = @"C:\Source\Repos\Project Memphis\ProjectMemphis\oneflow.json";
            return JsonConvert.DeserializeObject<OneFlowGraph>(File.ReadAllText(json)).ToStory();
        }
    }
}