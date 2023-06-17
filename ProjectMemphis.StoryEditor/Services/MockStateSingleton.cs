using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectMemphis.Data;

namespace ProjectMemphis.StoryEditor.Services
{
    public class MockStateSingleton : IStateSingleton
    {
        public MockStateSingleton()
        {
            States = new List<State>();
            States.Add(new State { Guid = new Guid("9d95792b-107b-41e2-922a-7cb24bf52740") });
        }

        public List<State> States
        {
            get; set;
        }
    }
}
