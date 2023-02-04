using AutoMapper;
using ConsumeApiTest.DataAccess.ConfiguratonSettings;
using ConsumeApiTest.DataAccess.Models;
using ConsumeApiTest.Models;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ConsumeApiTest.DataAccess.Services.Api
{

    public interface ISSMWorkFlowInstanceActionHistory
    {
        Task<Guid> Add(CreateUpdateWorkFlowInstanceActionHistory workFlowInstanceActionHistory);
        //Task Delete(Guid workflowId);
        Task<WorkFlowInstanceActionHistoryViewModel> Get(Guid workflowInstanceActionHistoryId);
        Task<List<WorkFlowInstanceActionHistoryViewModel>> GetAllByWorkflowInstanceId(Guid workFlowInstanceId);
        //Task<WorkFlowInstanceViewModel> Update(CreateUpdateWorkFlowInstance workFlow, Guid workflowId);
    }


    public class SSMWorkFlowInstanceActionHistory : ISSMWorkFlowInstanceActionHistory
    {
        private const string API_REQUEST_HEADER_NAME = "SSMWorkFlow-Subscription-Key";

        private readonly SSMWorkFlowSettings _ssmWorkFlowSettings;
        private readonly IMapper _mapper;

        public SSMWorkFlowInstanceActionHistory(
            IOptionsMonitor<SSMWorkFlowSettings> ssmWorkFlowSettings,
            IMapper mapper
            )
        {
            _ssmWorkFlowSettings = ssmWorkFlowSettings.CurrentValue;
            _mapper = mapper;

        }


        public async Task<Guid> Add(CreateUpdateWorkFlowInstanceActionHistory createWorkFlowInstanceActionHistory)
        {
            var workflowInstanceActionHistory = _mapper.Map<WorkflowInstanceActionHistory>(createWorkFlowInstanceActionHistory);

            try
            {
                var workflowInstanceActionHistoryId = Guid.Empty;

                var returnValue = await _ssmWorkFlowSettings.BaseApiUrl
                    .AppendPathSegment("WorkFlowInstanceActionHistory")
                    //.WithHeader(API_REQUEST_HEADER_NAME, _ssmWorkFlowInstanceSettings.ApiKey)
                    .PostJsonAsync(workflowInstanceActionHistory)
                    .ReceiveString();

                var deserialized = JsonConvert.DeserializeObject<Response<Guid>>(returnValue);

                if (deserialized != null)
                {
                    workflowInstanceActionHistoryId = deserialized.Result;
                }

                return workflowInstanceActionHistoryId;
            }
            catch (FlurlHttpException ex)
            {
                var exceptionResponse = await ex.GetResponseStringAsync();
                throw new Exception($"Failed attempting to send add request to SSMWorkFlowInstanceActionHistory. {exceptionResponse}");
            }
        }

        public async Task<WorkFlowInstanceActionHistoryViewModel> Get(Guid workflowInstanceActionHistoryId)
        {
            try
            {
                var workFlowInstanceActionHistory = new WorkFlowInstanceActionHistoryViewModel();

                var url = _ssmWorkFlowSettings.BaseApiUrl
                        .AppendPathSegment("WorkFlowInstanceActionHistory")
                        .AppendPathSegment($"{workflowInstanceActionHistoryId}");

                var workflowId = new Guid("B8B60FEF-F5A3-ED11-BE78-60E32BCDF739");

                var nresponse = await _ssmWorkFlowSettings.BaseApiUrl
                    .AppendPathSegment("WorkFlow")
                    .AppendPathSegment($"{workflowId}")
                    .GetJsonAsync<Response<WorkFlowViewModel>>();

                var response = await _ssmWorkFlowSettings.BaseApiUrl
                        .AppendPathSegment("WorkFlowInstanceActionHistory")
                        .AppendPathSegment($"{workflowInstanceActionHistoryId}")
                        .GetJsonAsync<Response<WorkFlowInstanceActionHistoryViewModel>>();

                if (response != null)
                {
                    workFlowInstanceActionHistory = response.Result;
                }

                return _mapper.Map<WorkFlowInstanceActionHistoryViewModel>(workFlowInstanceActionHistory);
            }
            catch (FlurlHttpException ex)
            {
                var exceptionResponse = await ex.GetResponseStringAsync();
                throw new Exception($"Failed attempting to send get request to SSMWorkFlowInstanceActionHistory. {exceptionResponse}");
            }
        }

        public async Task<List<WorkFlowInstanceActionHistoryViewModel>> GetAllByWorkflowInstanceId(Guid workFlowInstanceId)
        {
            try
            {
                var workFlowInstanceActionHistoryViewModel = new List<WorkFlowInstanceActionHistoryViewModel>();

                try
                {
                    var response = await _ssmWorkFlowSettings.BaseApiUrl
                            .AppendPathSegment("WorkFlowInstanceActionHistory")
                            .SetQueryParam($"WorkflowInstanceID={workFlowInstanceId}")
                            .GetJsonAsync<Response<dynamic>>();

                    var responseObject = JsonConvert.SerializeObject(response.Result);
                    var results = JsonConvert.DeserializeObject<List<WorkFlowInstanceActionHistoryViewModel>>(responseObject);

                    if (results != null)
                    {
                        foreach (var result in results)
                        {
                            workFlowInstanceActionHistoryViewModel.Add(result);
                        }

                    }
                }
                catch (Exception ex)
                {

                    throw;
                }

                return workFlowInstanceActionHistoryViewModel;

            }
            catch (FlurlHttpException ex)
            {
                var exceptionResponse = await ex.GetResponseStringAsync();
                throw new Exception($"Failed attempting to send get all request to SSMWorkFlowInstanceActionHistory. {exceptionResponse}");
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
