namespace ConsumeApiTest.Models
{
    public class WorkFlowStepResponderViewModel
    {
        public Guid WorkflowStepID { get; set; }
        public Guid ResponderID { get; set; }
        public bool isGroup { get; set; }
        public string Responder { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime Updated { get; set; }
        public string UpdatedBy { get; set; }

    }
}
