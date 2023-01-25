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

    public interface ISSMWorkFlowStakeholder
    {
        Task<Guid> Add(CreateUpdateWorkFlowStakeholder workFlowStakeholder);
        //Task Delete(Guid workflowId);
        Task<WorkFlowViewModel> Get(Guid workflowID);
        //Task<WorkFlowViewModel> Update(CreateUpdateWorkFlow workFlow, Guid workflowId);
    }
    public class SSMWorkFlowStakeholder : ISSMWorkFlowStakeholder
    {
        private const string API_REQUEST_HEADER_NAME = "SSMWorkFlow-Subscription-Key";

        private readonly SSMWorkFlowSettings _ssmWorkFlowSettings;
        private readonly IMapper _mapper;

        public SSMWorkFlowStakeholder(
            IOptionsMonitor<SSMWorkFlowSettings> ssmWorkFlowSettings,
            IMapper mapper
            )
        {
            _ssmWorkFlowSettings = ssmWorkFlowSettings.CurrentValue;
            _mapper = mapper;

        }

               
        public async Task<Guid> Add(CreateUpdateWorkFlowStakeholder createWorkFlowStakeholder)
        {
            var workflowStakeholder = _mapper.Map<WorkflowStakeholder>(createWorkFlowStakeholder);

            try
            {
                var workflowId = Guid.Empty;

                var returnValue = await _ssmWorkFlowSettings.BaseApiUrl
                    .AppendPathSegment("WorkFlowStakeholder")
                    //.WithHeader(API_REQUEST_HEADER_NAME, _ssmWorkFlowSettings.ApiKey)
                    .PostJsonAsync(workflowStakeholder)
                    .ReceiveString();

                var deserialized = JsonConvert.DeserializeObject<Response<Guid>>(returnValue);

                if(deserialized != null)
                {
                    workflowId = deserialized.Result;
                }

                return workflowId;
            }
            catch (FlurlHttpException ex)
            {
                var exceptionResponse = await ex.GetResponseStringAsync();
                throw new Exception($"Failed attempting to send add request to SSMWorkFlow. {exceptionResponse}");
            }
        }

        public async Task<WorkFlowViewModel> Get(Guid workflowID)
        {
            try
            {
                var workFlowViewModel = new WorkFlowViewModel();

                var returnValue = await _ssmWorkFlowSettings.BaseApiUrl
                    .AppendPathSegment("WorkFlow")
                    //.WithHeader(API_REQUEST_HEADER_NAME, _ssmWorkFlowSettings.ApiKey)
                    .PostJsonAsync(workflowID)
                    .ReceiveString();

                var deserialized = JsonConvert.DeserializeObject<Response<WorkFlowViewModel>>(returnValue);

                if (deserialized != null)
                {
                    workFlowViewModel = deserialized.Result;
                }

                return workFlowViewModel;
            }
            catch (FlurlHttpException ex)
            {
                var exceptionResponse = await ex.GetResponseStringAsync();
                throw new Exception($"Failed attempting to send get request to SSMWorkFlow. {exceptionResponse}");
            }
        }


        //public Task Delete(Guid workflowId)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<WorkFlowViewModel> Get(Guid workflowID)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<WorkFlowViewModel> Update(CreateUpdateWorkFlow workFlow, Guid workflowId)
        //{
        //    throw new NotImplementedException();
        //}
    }


}
