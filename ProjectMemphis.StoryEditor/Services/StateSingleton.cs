using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectMemphis.Data;
using ProjectMemphis.StoryEditor.Services;

namespace ProjectMemphis.StoryEditor.Services
{
    public class StateSingleton : IStateSingleton
    {
        public StateSingleton()
        {
            States = new List<State>();
        }

        public List<State> States
        {
            get; set;
        }
    }
}
