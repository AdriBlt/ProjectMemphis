namespace OneFlow.Data
{
    public class OneFlowNode
    {
        public OneFlowStepType StepType { get; set; }

        public string Id { get; set; }

        public string Text { get; set; }

        public string Question { get; set; }

        public OneFlowAnswer Ok { get; set; }

        public OneFlowAnswer Yes { get; set; }

        public OneFlowAnswer No { get; set; }

        public OneFlowAnswer Idk { get; set; }
    }

    public enum OneFlowStepType
    {
        QuestionStep,
        SimpleStep
    }
}