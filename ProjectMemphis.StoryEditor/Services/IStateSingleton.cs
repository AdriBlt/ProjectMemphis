using ProjectMemphis.Data;
using System.Collections.Generic;

namespace ProjectMemphis.StoryEditor.Services
{
    public interface IStateSingleton
    {
        List<State> States { get; set; }
    }
}
