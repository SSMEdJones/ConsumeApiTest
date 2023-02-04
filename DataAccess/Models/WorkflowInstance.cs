namespace ConsumeApiTest.DataAccess.Models
{
    public class WorkflowInstance
    {
        //public WorkflowInstance()
        //{
        //    WorkflowInstanceActionHistory = new HashSet<WorkflowInstanceActionHistory>();
        //}

        public Guid WorkflowInstanceID { get; set; }
        public Guid WorkflowID { get; set; }
        public Guid CurrentWorkflowStepID { get; set; }
        public string CurrentWorkflowState { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Updated { get; set; }
        public string UpdatedBy { get; set; }

        //public virtual Workflow Workflow { get; set; }
        //public virtual ICollection<WorkflowInstanceActionHistory> WorkflowInstanceActionHistory { get; set; }
    }
}
