namespace ConsumeApiTest.DataAccess.Models
{
    public class WorkflowTest
    {
        public int WorkflowInstanceID { get; set; }
        public int WorkflowStepID { get; set; }
        public int? OptionID { get; set; }
        public string Action { get; set; }
        public DateTime Completed { get; set; }
        public string CompletedBy { get; set; }
    }
}
