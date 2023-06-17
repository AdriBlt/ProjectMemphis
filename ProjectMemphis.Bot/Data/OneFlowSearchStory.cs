using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFlow.Clients;
using ProjectMemphis.Data;

namespace ProjectMemphis.Bot.Data
{
    public class OneFlowSearchState : State
    {
        private readonly IOneFlowClient _client;

        public OneFlowSearchState(Node stateNode, IOneFlowClient oneFlowClient)
        {
            _client = oneFlowClient;
            Node = stateNode;
            Message = "Type your search query:";
            UserInputExpected = true;            
        }
    }
}