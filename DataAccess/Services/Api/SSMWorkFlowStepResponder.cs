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

    public interface ISSMWorkFlowStepResponder
    {
        Task<Guid> Add(CreateUpdateWorkFlowStepResponder workFlow);
        //Task Delete(Guid workflowId);
        Task<WorkFlowStepResponderViewModel> Get(Guid workflowID);
        //Task<WorkFlowStepResponderViewModel> Update(CreateUpdateWorkFlowStepResponder workFlow, Guid workflowId);
    }
    public class SSMWorkFlowStepResponder : ISSMWorkFlowStepResponder
    {
        private const string API_REQUEST_HEADER_NAME = "SSMWorkFlow-Subscription-Key";

        private readonly SSMWorkFlowSettings _ssmWorkFlowSettings;
        private readonly IMapper _mapper;

        public SSMWorkFlowStepResponder(
            IOptionsMonitor<SSMWorkFlowSettings> ssmWorkFlowSettings,
            IMapper mapper
            )
        {
            _ssmWorkFlowSettings = ssmWorkFlowSettings.CurrentValue;
            _mapper = mapper;

        }

               
        public async Task<Guid> Add(CreateUpdateWorkFlowStepResponder createWorkFlowStepResponder)
        {
            var workflowStepResponder = _mapper.Map<WorkflowStepResponder>(createWorkFlowStepResponder);

            try
            {
                var optionId = Guid.Empty;

                var returnValue = await _ssmWorkFlowSettings.BaseApiUrl
                    .AppendPathSegment("WorkFlowStepResponder")
                    //.WithHeader(API_REQUEST_HEADER_NAME, _ssmWorkFlowStepResponderSettings.ApiKey)
                    .PostJsonAsync(workflowStepResponder)
                    .ReceiveString();

                var deserialized = JsonConvert.DeserializeObject<Response<Guid>>(returnValue);

                if(deserialized != null)
                {
                    optionId = deserialized.Result;
                }

                return optionId;
            }
            catch (FlurlHttpException ex)
            {
                var exceptionResponse = await ex.GetResponseStringAsync();
                throw new Exception($"Failed attempting to send add request to SSMWorkFlowStepResponder. {exceptionResponse}");
            }
        }

        public async Task<WorkFlowStepResponderViewModel> Get(Guid optionId)
        {
            try
            {
                var workFlowStepResponderViewModel = new WorkFlowStepResponderViewModel();

                var returnValue = await _ssmWorkFlowSettings.BaseApiUrl
                    .AppendPathSegment("WorkFlowStepResponder")
                    //.WithHeader(API_REQUEST_HEADER_NAME, _ssmWorkFlowStepResponderSettings.ApiKey)
                    .PostJsonAsync(optionId)
                    .ReceiveString();

                var deserialized = JsonConvert.DeserializeObject<Response<WorkFlowStepResponderViewModel>>(returnValue);

                if (deserialized != null)
                {
                    workFlowStepResponderViewModel = deserialized.Result;
                }

                return workFlowStepResponderViewModel;
            }
            catch (FlurlHttpException ex)
            {
                var exceptionResponse = await ex.GetResponseStringAsync();
                throw new Exception($"Failed attempting to send get request to SSMWorkFlowStepResponder. {exceptionResponse}");
            }
        }


        //public Task Delete(Guid workflowId)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<WorkFlowStepResponderViewModel> Get(Guid workflowID)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<WorkFlowStepResponderViewModel> Update(CreateUpdateWorkFlowStepResponder workFlow, Guid workflowId)
        //{
        //    throw new NotImplementedException();
        //}
    }


}
