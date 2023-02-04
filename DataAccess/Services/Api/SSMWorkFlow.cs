﻿using AutoMapper;
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

    public interface ISSMWorkFlow
    {
        Task<Guid> Add(CreateUpdateWorkFlow workFlow);
        Task Delete(Guid workflowId);
        Task<WorkFlowViewModel> Get(Guid workflowID);
        Task<List<WorkFlowViewModel>>GetAll(bool activeOnly);
        Task<WorkFlowViewModel> Update(CreateUpdateWorkFlow workFlow, Guid workflowId);
    }

    public class SSMWorkFlow : ISSMWorkFlow
    {
        private const string API_REQUEST_HEADER_NAME = "SSMWorkFlow-Subscription-Key";

        private readonly SSMWorkFlowSettings _ssmWorkFlowSettings;
        private readonly IMapper _mapper;

        public SSMWorkFlow(
            IOptionsMonitor<SSMWorkFlowSettings> ssmWorkFlowSettings,
            IMapper mapper
            )
        {
            _ssmWorkFlowSettings = ssmWorkFlowSettings.CurrentValue;
            _mapper = mapper;

        }

               
        public async Task<Guid> Add(CreateUpdateWorkFlow createWorkFlow)
        {
            var workflow = _mapper.Map<Workflow>(createWorkFlow);

            try
            {
                var workflowId = Guid.Empty;

                var response = await _ssmWorkFlowSettings.BaseApiUrl
                    .AppendPathSegment("WorkFlow")
                    //.WithHeader(API_REQUEST_HEADER_NAME, _ssmWorkFlowSettings.ApiKey)
                    .PostJsonAsync(workflow)
                    .ReceiveJson<Response<Guid>>();

                workflowId = response.Result;

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

                var response = await _ssmWorkFlowSettings.BaseApiUrl
                        .AppendPathSegment("WorkFlow")
                        .AppendPathSegment($"{workflowID}")
                        .GetJsonAsync<Response<dynamic>>();

                var responseObject = JsonConvert.SerializeObject(response.Result);
                var results = JsonConvert.DeserializeObject<WorkFlowViewModel>(responseObject);

                if (results != null)
                {
                    workFlowViewModel = results;
                }

                return _mapper.Map<WorkFlowViewModel>(workFlowViewModel);
            }
            catch (FlurlHttpException ex)
            {
                var exceptionResponse = await ex.GetResponseStringAsync();
                throw new Exception($"Failed attempting to send get request to SSMWorkFlow. {exceptionResponse}");
            }
        }

        public async Task<List<WorkFlowViewModel>> GetAll(bool activeOnly)
        {
            try
            {
                var workFlowViewModel = new List<WorkFlowViewModel>();

                    var response = await _ssmWorkFlowSettings.BaseApiUrl
                            .AppendPathSegment("WorkFlow")
                            .SetQueryParam($"ActiveOnly={activeOnly}")
                            .GetJsonAsync<Response<dynamic>>();

                    var responseObject = JsonConvert.SerializeObject(response.Result);
                    var results = JsonConvert.DeserializeObject<List<WorkFlowViewModel>>(responseObject);

                    if (results != null)
                    {
                        foreach (var result in results)
                        {
                            workFlowViewModel.Add(result);
                        }

                    }

                return workFlowViewModel;

            }
            catch (FlurlHttpException ex)
            {
                var exceptionResponse = await ex.GetResponseStringAsync();
                throw new Exception($"Failed attempting to send get all request to SSMWorkFlow. {exceptionResponse}");
            }

        }

        public async Task Delete(Guid workflowId)
        {
            await _ssmWorkFlowSettings.BaseApiUrl
                        .AppendPathSegment("WorkFlow")
                        .AppendPathSegment($"{workflowId}")
                        .DeleteAsync();

        }

        public async Task<WorkFlowViewModel> Update(CreateUpdateWorkFlow workFlow, Guid workflowId)
        {
            var workFlowViewModel = new WorkFlowViewModel();

            try
            {
                var response = await _ssmWorkFlowSettings.BaseApiUrl
                        .AppendPathSegment("WorkFlow")
                        .AppendPathSegment($"{workflowId}")
                        .PutJsonAsync(workFlow)
                        .ReceiveJson<Response<WorkFlowViewModel>>();
                
                var responseObject = JsonConvert.SerializeObject(response.Result);
                var results = JsonConvert.DeserializeObject<WorkFlowViewModel>(responseObject);

                if (results != null)
                {
                    workFlowViewModel = results;
                }

                return _mapper.Map<WorkFlowViewModel>(workFlowViewModel);

            }
            catch (FlurlHttpException ex)
            {
                var exceptionResponse = await ex.GetResponseStringAsync();
                throw new Exception($"Failed attempting to send update to SSMWorkFlow. {exceptionResponse}");
            }
        }
    }


}
