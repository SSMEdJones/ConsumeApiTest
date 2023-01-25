namespace ConsumeApiTest.DataAccess.Models
{
    public class Workflow
    {
        //public Workflow()
        //{
        //    WorkflowInstance = new HashSet<WorkflowInstance>();
        //    WorkflowStakeholder = new HashSet<WorkflowStakeholder>();
        //    WorkflowStep = new HashSet<WorkflowStep>();
        //}

        public Guid WorkflowID { get; set; }
        public string WorkflowName { get; set; }
        public string WorkflowDescription { get; set; }
        public string StakeholderNotificationType { get; set; }
        public string CompleteMessage { get; set; }
        public string CancelledMessage { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime Updated { get; set; }
        public string UpdatedBy { get; set; }

        //public virtual ICollection<WorkflowInstance> WorkflowInstance { get; set; }
        //public virtual ICollection<WorkflowStakeholder> WorkflowStakeholder { get; set; }
        //public virtual ICollection<WorkflowStep> WorkflowStep { get; set; }
    }
}
