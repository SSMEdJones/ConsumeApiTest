using AutoMapper;
using ConsumeApiTest.DataAccess.ConfiguratonSettings;
using ConsumeApiTest.DataAccess.Models;
using ConsumeApiTest.Models;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;

namespace ConsumeApiTest.DataAccess.Services.Api
{

    public interface ISSMWorkFlowStep
    {
        Task<Guid> Add(CreateUpdateWorkFlowStep workFlow);
        //Task Delete(Guid workflowId);
        Task<WorkFlowStepViewModel> Get(Guid workflowID);
        //Task<WorkFlowStepViewModel> Update(CreateUpdateWorkFlowStep workFlow, Guid workflowId);
    }
    public class SSMWorkFlowStep : ISSMWorkFlowStep
    {
        private const string API_REQUEST_HEADER_NAME = "SSMWorkFlow-Subscription-Key";

        private readonly SSMWorkFlowSettings _ssmWorkFlowSettings;
        private readonly IMapper _mapper;

        public SSMWorkFlowStep(
            IOptionsMonitor<SSMWorkFlowSettings> ssmWorkFlowSettings,
            IMapper mapper
            )
        {
            _ssmWorkFlowSettings = ssmWorkFlowSettings.CurrentValue;
            _mapper = mapper;

        }

               
        public async Task<Guid> Add(CreateUpdateWorkFlowStep createWorkFlowStep)
        {
            var workflowStep = _mapper.Map<WorkflowStep>(createWorkFlowStep);

            try
            {
                var workflowStepId = Guid.Empty;

                var returnValue = await _ssmWorkFlowSettings.BaseApiUrl
                    .AppendPathSegment("WorkFlowStep")
                    //.WithHeader(API_REQUEST_HEADER_NAME, _ssmWorkFlowStepSettings.ApiKey)
                    .PostJsonAsync(workflowStep)
                    .ReceiveString();

                var deserialized = JsonConvert.DeserializeObject<Response<Guid>>(returnValue);

                if(deserialized != null)
                {
                    workflowStepId = deserialized.Result;
                }

                return workflowStepId;
            }
            catch (FlurlHttpException ex)
            {
                var exceptionResponse = await ex.GetResponseStringAsync();
                throw new Exception($"Failed attempting to send add request to SSMWorkFlowStep. {exceptionResponse}");
            }
        }

        public async Task<WorkFlowStepViewModel> Get(Guid workflowStepID)
        {
            try
            {
                var workFlowViewModel = new WorkFlowStepViewModel();

                var returnValue = await _ssmWorkFlowSettings.BaseApiUrl
                    .AppendPathSegment("WorkFlowStep")
                    //.WithHeader(API_REQUEST_HEADER_NAME, _ssmWorkFlowStepSettings.ApiKey)
                    .PostJsonAsync(workflowStepID)
                    .ReceiveString();

                var deserialized = JsonConvert.DeserializeObject<Response<WorkFlowStepViewModel>>(returnValue);

                if (deserialized != null)
                {
                    workFlowViewModel = deserialized.Result;
                }

                return workFlowViewModel;
            }
            catch (FlurlHttpException ex)
            {
                var exceptionResponse = await ex.GetResponseStringAsync();
                throw new Exception($"Failed attempting to send get request to SSMWorkFlowStep. {exceptionResponse}");
            }
        }


        //public Task Delete(Guid workflowId)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<WorkFlowStepViewModel> Get(Guid workflowID)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<WorkFlowStepViewModel> Update(CreateUpdateWorkFlowStep workFlow, Guid workflowId)
        //{
        //    throw new NotImplementedException();
        //}
    }


}
