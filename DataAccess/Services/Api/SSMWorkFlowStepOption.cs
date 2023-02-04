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

    public interface ISSMWorkFlowStepOption
    {
        Task<Guid> Add(CreateUpdateWorkFlowStepOption workFlowStepOptio);
        //Task Delete(Guid workflowId);
        Task<WorkFlowStepOptionViewModel> Get(Guid workflowStepOptionID);
        //Task<WorkFlowStepOptionViewModel> Update(CreateUpdateWorkFlowStepOption workFlow, Guid workflowId);
    }
    public class SSMWorkFlowStepOption : ISSMWorkFlowStepOption
    {
        private const string API_REQUEST_HEADER_NAME = "SSMWorkFlow-Subscription-Key";

        private readonly SSMWorkFlowSettings _ssmWorkFlowSettings;
        private readonly IMapper _mapper;

        public SSMWorkFlowStepOption(
            IOptionsMonitor<SSMWorkFlowSettings> ssmWorkFlowSettings,
            IMapper mapper
            )
        {
            _ssmWorkFlowSettings = ssmWorkFlowSettings.CurrentValue;
            _mapper = mapper;

        }

               
        public async Task<Guid> Add(CreateUpdateWorkFlowStepOption createWorkFlowStepOption)
        {
            var workflowStepOption = _mapper.Map<WorkflowStepOption>(createWorkFlowStepOption);

            try
            {
                var optionId = Guid.Empty;

                var returnValue = await _ssmWorkFlowSettings.BaseApiUrl
                    .AppendPathSegment("WorkFlowStepOption")
                    //.WithHeader(API_REQUEST_HEADER_NAME, _ssmWorkFlowStepOptionSettings.ApiKey)
                    .PostJsonAsync(workflowStepOption)
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
                throw new Exception($"Failed attempting to send add request to SSMWorkFlowStepOption. {exceptionResponse}");
            }
        }

        public async Task<WorkFlowStepOptionViewModel> Get(Guid optionId)
        {
            try
            {
                var workFlowStepOptionViewModel = new WorkFlowStepOptionViewModel();

                var returnValue = await _ssmWorkFlowSettings.BaseApiUrl
                    .AppendPathSegment("WorkFlowStepOption")
                    //.WithHeader(API_REQUEST_HEADER_NAME, _ssmWorkFlowStepOptionSettings.ApiKey)
                    .PostJsonAsync(optionId)
                    .ReceiveString();

                var deserialized = JsonConvert.DeserializeObject<Response<WorkFlowStepOptionViewModel>>(returnValue);

                if (deserialized != null)
                {
                    workFlowStepOptionViewModel = deserialized.Result;
                }

                return workFlowStepOptionViewModel;
            }
            catch (FlurlHttpException ex)
            {
                var exceptionResponse = await ex.GetResponseStringAsync();
                throw new Exception($"Failed attempting to send get request to SSMWorkFlowStepOption. {exceptionResponse}");
            }
        }


        //public Task Delete(Guid workflowId)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<WorkFlowStepOptionViewModel> Get(Guid workflowID)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<WorkFlowStepOptionViewModel> Update(CreateUpdateWorkFlowStepOption workFlow, Guid workflowId)
        //{
        //    throw new NotImplementedException();
        //}
    }


}
