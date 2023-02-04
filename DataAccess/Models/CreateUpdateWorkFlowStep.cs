﻿namespace ConsumeApiTest.DataAccess.Models
{
    public class CreateUpdateWorkFlowStep
    {
        public Guid WorkflowID { get; set; }
        public string StepName { get; set; }
        public string StepDescription { get; set; }
        public bool isRoot { get; set; }
        public bool supervisorStep { get; set; }
        public string ResponderMessage { get; set; }
        public string ResponderWarningMessage { get; set; }
        public string StakeholderMessage { get; set; }
        public string StakeholderWarningMessage { get; set; }
        public int? DaysTillDue { get; set; }
        public int? WarningDays1 { get; set; }
        public int? WarningDays2 { get; set; }
        public bool WarningDays2Daily { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime Updated { get; set; }
        public string UpdatedBy { get; set; }

    }
}
