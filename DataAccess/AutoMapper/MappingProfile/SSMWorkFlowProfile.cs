using AutoMapper;
using ConsumeApiTest.DataAccess.Models;
using ConsumeApiTest.Models;

namespace ConsumeApiTest.DataAccess.AutoMapper.MappingProfile
{
    public class SSMWorkflowProfile : Profile
    {
        public SSMWorkflowProfile()
        {

            //WorkFlow
            CreateMap<WorkFlowViewModel, CreateUpdateWorkFlow>();
            CreateMap<CreateUpdateWorkFlow, Workflow>();

            ////WorkFlowStakeHolder
            CreateMap<WorkFlowStakeholderViewModel, CreateUpdateWorkFlowStakeholder>();
            CreateMap<CreateUpdateWorkFlowStakeholder, WorkflowStakeholder>();

            ////WorkFlowStep
            CreateMap<WorkFlowStepViewModel, CreateUpdateWorkFlowStep>();
            CreateMap<CreateUpdateWorkFlowStep, WorkflowStep>();

            ////WorkFlowInstance
            CreateMap<WorkFlowInstanceViewModel, CreateUpdateWorkFlowInstance>();
            CreateMap<CreateUpdateWorkFlowInstance, WorkflowInstance>();

            ////WorkFlowStepOption
            CreateMap<WorkFlowStepOptionViewModel, CreateUpdateWorkFlowStepOption>();
            CreateMap<CreateUpdateWorkFlowStepOption, WorkflowStepOption>();

            ////WorkFlowStepResponder
            CreateMap<WorkFlowStepResponderViewModel, CreateUpdateWorkFlowStepResponder>();
            CreateMap<CreateUpdateWorkFlowStepResponder, WorkflowStepResponder>();

        
            ////WorkFlowInstanceActionHistory
            //CreateMap<CreateUpdateWorkFlowInstanceActionHistory, WorkflowInstanceActionHistory>();
            //CreateMap<WorkFlowInstanceActionHistory, WorkflowInstanceActionHistory>();
            //CreateMap<WorkflowInstanceActionHistory, WorkFlowInstanceActionHistory>();


        }

    }
}
