using System;

namespace OneFlow.Data
{
    public class OneFlowAnswer
    {
        public OneFlowAnswerType AnswerType { get; set; }

        public string StepId { get; set; }

        public string Url { get; set; }
        
        public Guid GraphId { get; set; }

        public string CallbackStepId { get; set; }

        public bool IsSuccess { get; set; }
    }

    public enum OneFlowAnswerType
    {
        StepAnswer,
        UrlAnswer,
        GraphAnswer,
        FinalAnswer
    }
}