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

    public interface ISSMWorkFlowInstance
    {
        Task<Guid> Add(CreateUpdateWorkFlowInstance workFlow);
        //Task Delete(Guid workflowId);
        Task<WorkFlowInstanceViewModel> Get(Guid workflowID);
        //Task<WorkFlowInstanceViewModel> Update(CreateUpdateWorkFlowInstance workFlow, Guid workflowId);
    }
    public class SSMWorkFlowInstance : ISSMWorkFlowInstance
    {
        private const string API_REQUEST_HEADER_NAME = "SSMWorkFlow-Subscription-Key";

        private readonly SSMWorkFlowSettings _ssmWorkFlowSettings;
        private readonly IMapper _mapper;

        public SSMWorkFlowInstance(
            IOptionsMonitor<SSMWorkFlowSettings> ssmWorkFlowSettings,
            IMapper mapper
            )
        {
            _ssmWorkFlowSettings = ssmWorkFlowSettings.CurrentValue;
            _mapper = mapper;

        }

               
        public async Task<Guid> Add(CreateUpdateWorkFlowInstance createWorkFlowInstance)
        {
            var workflowInstance = _mapper.Map<WorkflowInstance>(createWorkFlowInstance);

            try
            {
                var workflowInstanceId = Guid.Empty;

                var returnValue = await _ssmWorkFlowSettings.BaseApiUrl
                    .AppendPathSegment("WorkFlowInstance")
                    //.WithHeader(API_REQUEST_HEADER_NAME, _ssmWorkFlowInstanceSettings.ApiKey)
                    .PostJsonAsync(workflowInstance)
                    .ReceiveString();

                var deserialized = JsonConvert.DeserializeObject<Response<Guid>>(returnValue);

                if(deserialized != null)
                {
                    workflowInstanceId = deserialized.Result;
                }

                return workflowInstanceId;
            }
            catch (FlurlHttpException ex)
            {
                var exceptionResponse = await ex.GetResponseStringAsync();
                throw new Exception($"Failed attempting to send add request to SSMWorkFlowInstance. {exceptionResponse}");
            }
        }

        public async Task<WorkFlowInstanceViewModel> Get(Guid workflowInstanceID)
        {
            try
            {
                var workFlowInstanceViewModel = new WorkFlowInstanceViewModel();

                var returnValue = await _ssmWorkFlowSettings.BaseApiUrl
                    .AppendPathSegment("WorkFlowInstance")
                    //.WithHeader(API_REQUEST_HEADER_NAME, _ssmWorkFlowInstanceSettings.ApiKey)
                    .PostJsonAsync(workflowInstanceID)
                    .ReceiveString();

                var deserialized = JsonConvert.DeserializeObject<Response<WorkFlowInstanceViewModel>>(returnValue);

                if (deserialized != null)
                {
                    workFlowInstanceViewModel = deserialized.Result;
                }

                return workFlowInstanceViewModel;
            }
            catch (FlurlHttpException ex)
            {
                var exceptionResponse = await ex.GetResponseStringAsync();
                throw new Exception($"Failed attempting to send get request to SSMWorkFlowInstance. {exceptionResponse}");
            }
        }


        //public Task Delete(Guid workflowId)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<WorkFlowInstanceViewModel> Get(Guid workflowID)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<WorkFlowInstanceViewModel> Update(CreateUpdateWorkFlowInstance workFlow, Guid workflowId)
        //{
        //    throw new NotImplementedException();
        //}
    }


}
