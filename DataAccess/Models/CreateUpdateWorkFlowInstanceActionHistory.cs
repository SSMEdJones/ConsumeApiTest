namespace ConsumeApiTest.DataAccess.Models
{
    public class CreateUpdateWorkFlowInstanceActionHistory
    {
        public Guid WorkflowInstanceID { get; set; }
        public Guid WorkflowStepID { get; set; }
        public Guid? OptionID { get; set; }
        public string Action { get; set; }
        public DateTime Completed { get; set; }
        public string CompletedBy { get; set; }

    }
}
