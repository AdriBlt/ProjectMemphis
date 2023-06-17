using System;
using System.Collections.Generic;
using ProjectMemphis.Data;

namespace ProjectMemphis.Bot.Data
{
    public class ConfirmationState : State
    {
        public ConfirmationState(
            Node stateNode, 
            Node sourceNode, 
            Node targetNode, 
            string message, 
            string title,
            string okOption = "OK",
            string cancelOption = "Cancel")
        {
            Node = stateNode;
            Message = message;
            Events = new List<Event>
            {
                new OptionsEvent
                {
                    Title = title,
                    Options = new List<Option>
                    {
                        new StateOption { Text = cancelOption, TargetNode = sourceNode },
                        new StateOption { Text = okOption, TargetNode = targetNode }
                    }
                }
            };
        }
    }
}