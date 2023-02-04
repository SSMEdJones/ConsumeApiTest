using AutoMapper;
using ConsumeApiTest.DataAccess.ConfiguratonSettings;
using ConsumeApiTest.DataAccess.Models;
using ConsumeApiTest.Models;
using Flurl;
using Flurl.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ConsumeApiTest.DataAccess.Services.Api
{

    public interface ISSMMWorkFlowTest
    {
        Task<int> Add(CreateUpdateWorkFlowTest workFlowTest);
        //Task Delete(int workflowId);
        Task<WorkFlowTestViewModel> Get(int workflowTestId);
        Task<List<WorkFlowTestViewModel>> GetAllByWorkflowInstanceId(int workFlowInstanceId);
        //Task<WorkFlowInstanceViewModel> Update(CreateUpdateWorkFlowInstance workFlow, int workflowId);
    }


    public class SSMMWorkFlowTest : ISSMMWorkFlowTest
    {
        private const string API_REQUEST_HEADER_NAME = "SSMWorkFlow-Subscription-Key";

        private readonly SSMWorkFlowSettings _ssmWorkFlowSettings;
        private readonly IMapper _mapper;

        public SSMMWorkFlowTest(
            IOptionsMonitor<SSMWorkFlowSettings> ssmWorkFlowSettings,
            IMapper mapper
            )
        {
            _ssmWorkFlowSettings = ssmWorkFlowSettings.CurrentValue;
            _mapper = mapper;

        }


        public async Task<int> Add(CreateUpdateWorkFlowTest createWorkFlowTest)
        {
            var workflowTest = _mapper.Map<WorkflowTest>(createWorkFlowTest);

            try
            {
                return await _ssmWorkFlowSettings.BaseApiUrl
                    .AppendPathSegment("WorkFlowTest")
                    //.WithHeader(API_REQUEST_HEADER_NAME, _ssmWorkFlowInstanceSettings.ApiKey)
                    .PostJsonAsync(workflowTest)
                    .ReceiveJson<int>();

                //id = returnValue.
                //var deserialized = JsonConvert.DeserializeObject<Response<int>>(returnValue);

                //if (deserialized != null)
                //{
                //    workflowTestId = deserialized.Result;
                //}

                //return id;
            }
            catch (FlurlHttpException ex)
            {
                var exceptionResponse = await ex.GetResponseStringAsync();
                throw new Exception($"Failed attempting to send add request to SSMWorkFlowTest. {exceptionResponse}");
            }
        }

        public async Task<WorkFlowTestViewModel> Get(int id)
        {
            try
            {
                //var workFlowTest = new WorkFlowTestViewModel();

                var workFlowTest = await _ssmWorkFlowSettings.BaseApiUrl
                        .AppendPathSegment("WorkFlowTest")
                        .AppendPathSegment($"{id}")
                        .GetJsonAsync<Response<WorkFlowTestViewModel>>();

                return workFlowTest.Result;

                //return await _ssmWorkFlowSettings.BaseApiUrl
                //        .AppendPathSegment("WorkFlowTest")
                //        .AppendPathSegment($"{id}")
                //        .GetJsonAsync<WorkFlowTestViewModel>();

                //var returnValue = await _ssmWorkFlowSettings.BaseApiUrl
                //        .AppendPathSegment("WorkFlowTest")
                //        .AppendPathSegment($"{id}")
                //        .GetJsonAsync<WorkFlowTestViewModel>();

                //var deserialized = JsonConvert.DeserializeObject<Response<WorkFlowTestViewModel>>(returnValue);

                //if (deserialized != null)
                //{
                //    workFlowTest = deserialized.Result;
                //}

                //return _mapper.Map<WorkFlowTestViewModel>(workFlowTest);
            }
            catch (FlurlHttpException ex)
            {
                var exceptionResponse = await ex.GetResponseStringAsync();
                throw new Exception($"Failed attempting to send get request to SSMWorkFlowTest. {exceptionResponse}");
            }
        }

        public async Task<List<WorkFlowTestViewModel>> GetAllByWorkflowInstanceId(int workFlowInstanceId)
        {
            try
            {
                var workFlowTestViewModel = new List<WorkFlowTestViewModel>();

                try
                {
                    var response = await _ssmWorkFlowSettings.BaseApiUrl
                            .AppendPathSegment("WorkFlowTest")
                            .SetQueryParam($"WorkflowInstanceID={workFlowInstanceId}")
                            .GetJsonAsync<Response<dynamic>>();

                    var responseObject = JsonConvert.SerializeObject(response.Result);
                    var results = JsonConvert.DeserializeObject<List<WorkFlowTestViewModel>>(responseObject);

                    if (results != null)
                    {
                        foreach (var result in results)
                        {
                            workFlowTestViewModel.Add(result);
                        }

                    }
                }
                catch (Exception ex)
                {

                    throw;
                }

                return workFlowTestViewModel;

            }
            catch (FlurlHttpException ex)
            {
                var exceptionResponse = await ex.GetResponseStringAsync();
                throw new Exception($"Failed attempting to send get all request to SSMWorkFlowTest. {exceptionResponse}");
            }

        }



        //public Task Delete(int workflowId)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<WorkFlowInstanceViewModel> Get(int workflowID)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<WorkFlowInstanceViewModel> Update(CreateUpdateWorkFlowInstance workFlow, int workflowId)
        //{
        //    throw new NotImplementedException();
        //}
    }


}
