using AutoMapper;
using ConsumeApiTest.DataAccess.ConfiguratonSettings;
using ConsumeApiTest.DataAccess.Models;
using ConsumeApiTest.DataAccess.Services.Api;
using ConsumeApiTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Diagnostics;

namespace ConsumeApiTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISSMWorkFlow _ssmMWorkFlow;
        private readonly ISSMWorkFlowStakeholder _ssmMWorkFlowStakeholder;
        private readonly ISSMWorkFlowStep _ssmMWorkFlowStep;
        private readonly ISSMWorkFlowStepOption _ssmMWorkFlowStepOption;
        private readonly ISSMWorkFlowStepResponder _ssmMWorkFlowStepResponder;
        private readonly ISSMWorkFlowInstance _ssmMWorkFlowInstance;
        private readonly IMapper _mapper;

        public HomeController(
            ILogger<HomeController> logger,
            ISSMWorkFlow ssmMWorkFlow,
            ISSMWorkFlowStakeholder ssmMWorkFlowStakeholder,
            ISSMWorkFlowStep ssmMWorkFlowStep,
            ISSMWorkFlowStepOption ssmMWorkFlowStepOption,
            ISSMWorkFlowStepResponder ssmMWorkFlowStepResponder,
            ISSMWorkFlowInstance ssmMWorkFlowInstance,
            IMapper mapper
            )
        {
            _logger = logger;
            _ssmMWorkFlow = ssmMWorkFlow;
            _ssmMWorkFlowStakeholder = ssmMWorkFlowStakeholder;
            _ssmMWorkFlowStep = ssmMWorkFlowStep;
            _ssmMWorkFlowStepOption = ssmMWorkFlowStepOption;
            _ssmMWorkFlowStepResponder = ssmMWorkFlowStepResponder;
            _ssmMWorkFlowInstance = ssmMWorkFlowInstance;
            _mapper = mapper;
        }

        // GET: Home
        public IActionResult Index()
        {            
            var model = new WorkFlowViewModel();
            return View(model);
        }

        public ActionResult Workflow()
        {
            var model = new WorkFlowViewModel
            {
                Created = DateTime.Now,
                CreatedBy = "TestUser",
                Updated = DateTime.Now,
                UpdatedBy = "TestUser"
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Workflow(WorkFlowViewModel workFlowViewModel)
        {
            var createWorkFlow = _mapper.Map<CreateUpdateWorkFlow>(workFlowViewModel);

            var response = _ssmMWorkFlow.Add(createWorkFlow);

            if (response != null)
            {
                workFlowViewModel.WorkflowID = response.Result;
            }

            return RedirectToAction("Workflow");

        }

        public ActionResult WorkflowStakeholder()
        {
            var model = new WorkFlowStakeholderViewModel
            {
                Created = DateTime.Now,
                CreatedBy = "TestUser",
                Updated = DateTime.Now,
                UpdatedBy = "TestUser"
            };

            return View(model);

        }
        
        [HttpPost]
        public ActionResult WorkflowStakeholder(WorkFlowStakeholderViewModel workFlowStakeHolderViewModel)
        {
            var createWorkFlowStakeholder = _mapper.Map<CreateUpdateWorkFlowStakeholder>(workFlowStakeHolderViewModel);

            var response = _ssmMWorkFlowStakeholder.Add(createWorkFlowStakeholder);

            if (response != null)
            {
                workFlowStakeHolderViewModel.StakeholderID = response.Result;
            }

            return RedirectToAction("WorkflowStakeholder");

        }

        public ActionResult WorkflowStep()
        {
            var model = new WorkFlowStepViewModel
            {
                Created = DateTime.Now,
                CreatedBy = "TestUser",
                Updated = DateTime.Now,
                UpdatedBy = "TestUser"
            };

            return View(model);

        }

        [HttpPost]
        public ActionResult WorkflowStep(WorkFlowStepViewModel workFlowStepViewModel)
        {
            var createWorkFlowStep = _mapper.Map<CreateUpdateWorkFlowStep>(workFlowStepViewModel);

            var response = _ssmMWorkFlowStep.Add(createWorkFlowStep);

            if (response != null)
            {
                workFlowStepViewModel.WorkflowStepID = response.Result;
            }

            return RedirectToAction("WorkflowStep");

        }

        public ActionResult WorkflowStepOption()
        {
            var model = new WorkFlowStepOptionViewModel
            {
                Created = DateTime.Now,
                CreatedBy = "TestUser",
                Updated = DateTime.Now,
                UpdatedBy = "TestUser"
            };

            return View(model);

        }

        [HttpPost]
        public ActionResult WorkflowStepOption(WorkFlowStepOptionViewModel workFlowStepOptionViewModel)
        {
            var createWorkFlowStepOption = _mapper.Map<CreateUpdateWorkFlowStepOption>(workFlowStepOptionViewModel);

            var response = _ssmMWorkFlowStepOption.Add(createWorkFlowStepOption);

            if (response != null)
            {
                workFlowStepOptionViewModel.OptionID = response.Result;
            }

            return RedirectToAction("WorkflowStepOption");

        }

        public ActionResult WorkflowStepResponder()
        {
            var model = new WorkFlowStepResponderViewModel
            {
                Created = DateTime.Now,
                CreatedBy = "TestUser",
                Updated = DateTime.Now,
                UpdatedBy = "TestUser"
            };

            return View(model);

        }

        [HttpPost]
        public ActionResult WorkflowStepResponder(WorkFlowStepResponderViewModel workFlowStepResponderViewModel)
        {
            var createWorkFlowStepResponder = _mapper.Map<CreateUpdateWorkFlowStepResponder>(workFlowStepResponderViewModel);

            var response = _ssmMWorkFlowStepResponder.Add(createWorkFlowStepResponder);

            if (response != null)
            {
                workFlowStepResponderViewModel.ResponderID = response.Result;
            }

            return RedirectToAction("WorkflowStepResponder");

        }

        public ActionResult WorkflowInstance()
        {
            var model = new WorkFlowInstanceViewModel
            {
                Created = DateTime.Now,
                CreatedBy = "TestUser",
                Updated = DateTime.Now,
                UpdatedBy = "TestUser"
            };

            return View(model);

        }

        [HttpPost]
        public ActionResult WorkflowInstance(WorkFlowInstanceViewModel workFlowSInstanceViewModel)
        {
            var createWorkFlowInstance = _mapper.Map<CreateUpdateWorkFlowInstance>(workFlowSInstanceViewModel);

            var response = _ssmMWorkFlowInstance.Add(createWorkFlowInstance);

            if (response != null)
            {
                workFlowSInstanceViewModel.WorkflowInstanceID = response.Result;
            }

            return RedirectToAction("WorkflowInstance");

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}