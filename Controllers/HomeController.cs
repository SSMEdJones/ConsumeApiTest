using AutoMapper;
using ConsumeApiTest.DataAccess.Models;
using ConsumeApiTest.DataAccess.Services.Api;
using ConsumeApiTest.Models;
using Microsoft.AspNetCore.Mvc;
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
        private readonly ISSMWorkFlowInstanceActionHistory _ssmMWorkFlowInstanceActionHistory;
        private readonly ISSMMWorkFlowTest _ssmMWorkFlowTest;
        private readonly IMapper _mapper;

        public HomeController(
            ILogger<HomeController> logger,
            ISSMWorkFlow ssmMWorkFlow,
            ISSMWorkFlowStakeholder ssmMWorkFlowStakeholder,
            ISSMWorkFlowStep ssmMWorkFlowStep,
            ISSMWorkFlowStepOption ssmMWorkFlowStepOption,
            ISSMWorkFlowStepResponder ssmMWorkFlowStepResponder,
            ISSMWorkFlowInstance ssmMWorkFlowInstance,
            ISSMWorkFlowInstanceActionHistory ssmMWorkFlowInstanceActionHistory,
            ISSMMWorkFlowTest ssmMWorkFlowTest,
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
            _ssmMWorkFlowInstanceActionHistory = ssmMWorkFlowInstanceActionHistory;
            _ssmMWorkFlowTest = ssmMWorkFlowTest;
            _mapper = mapper;
        }

        // GET: Home
        public IActionResult Index()
        {            
            var model = new WorkFlowViewModel();
            return View(model);
        }

        #region Workflow
        public IActionResult WorkflowIndex()
        {
            //var model = new WorkFlowViewModel();
            return View();
        }

        public ActionResult Workflows(bool activeOnly)
        {

                var model = _ssmMWorkFlow
                         .GetAll(activeOnly)
                         .Result;

                return View(model);

        }

        public ActionResult WorkflowById(Guid workflowId, bool delete, bool edit)
        {

            var model= new WorkFlowViewModel();

            if (workflowId != Guid.Empty)
            {
                var response = _ssmMWorkFlow.Get(workflowId);

                if (response != null)
                {
                    model = response.Result;

                    if (delete)
                    {

                        return RedirectToAction("DeleteWorkflow", "Home", new { workflowId = model.WorkflowID });
                    }    
                    else if(edit)
                    {
                        return RedirectToAction("EditWorkflow", "Home", new { workflowId = model.WorkflowID });

                    }

                    return RedirectToAction("WorkflowDetails", model);
                }

            }

            return View(model);
        }

        public ActionResult WorkflowDetails(WorkFlowViewModel workFlowViewModel)
        {

            var model = workFlowViewModel;

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


            if (workFlowViewModel.Edit)
            {
                var response = _ssmMWorkFlow.Update(createWorkFlow, workFlowViewModel.WorkflowID);
                
                if (response != null)
                {
                    workFlowViewModel = response.Result;
                }
            }
            else
            {
                var response = _ssmMWorkFlow.Add(createWorkFlow);

                if (response != null)
                {
                    workFlowViewModel.WorkflowID = response.Result;
                }

            }

            return RedirectToAction("WorkflowIndex");

        }

        public ActionResult EditWorkflow(Guid workflowId)
        {
            var model = new WorkFlowViewModel();
            if (workflowId != Guid.Empty)
            {
                var response = _ssmMWorkFlow.Get(workflowId);

                if (response != null)
                {
                    model = response.Result;
                    model.Edit = true;

                    return View("Workflow",model);
                }

            }

            return RedirectToAction("WorkflowById", "Home", new { delete = false, edit = true });
        }

        public ActionResult DeleteWorkflow(Guid workflowId)
        {
            var model = new WorkFlowViewModel();
            if (workflowId != Guid.Empty)
            {
                var response = _ssmMWorkFlow.Get(workflowId);

                if (response != null)
                {
                    model = response.Result;

                    return View(model);
                }

            }

            return RedirectToAction("WorkflowById", "Home", new { delete = true, edit = false});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteWorkflow(WorkFlowViewModel workFlowViewModel)
        {
             _ssmMWorkFlow.Delete(workFlowViewModel.WorkflowID);

            return RedirectToAction("WorkflowIndex");
        }

        #endregion

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

        public ActionResult WorkflowInstanceActionHistory()
        {

            var workflowInstanceActionHistoryId = new Guid("A0925BD5-121D-EA11-A2D4-0050569736FD");

            try
            {
                var model = _ssmMWorkFlowInstanceActionHistory
                    .Get(workflowInstanceActionHistoryId)
                    .Result;    

                return View(model);
            }

            catch (Exception)
            {

                throw;
            }

        }

       

        public ActionResult WorkflowInstanceActionHistories()
        {

            var workflowInstanceId = new Guid("9FEDE8C0-121D-EA11-A2D4-0050569736FD");

            try
            {

                var model = _ssmMWorkFlowInstanceActionHistory
                         .GetAllByWorkflowInstanceId(workflowInstanceId)
                         .Result;
            
            return View(model);
            }

            catch (Exception)
            {

                throw;
            }

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


        public ActionResult WorkflowTest()
        {

            var id = 1;

            try
            {
                var model = _ssmMWorkFlowTest
                    .Get(id)
                    .Result;

                return View(model);
            }

            catch (Exception)
            {

                throw;
            }

        }

        public ActionResult WorkflowTests()
        {

            var workflowInstanceId = 1;

            try
            {

                var model = _ssmMWorkFlowTest
                         .GetAllByWorkflowInstanceId(workflowInstanceId)
                         .Result;

                return View(model);
            }

            catch (Exception)
            {

                throw;
            }

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

       
        //public Guid CompleteWorkflowInstanceAction(Guid InstanceID, Guid WorkflowStepID, Guid OptionID, string Action, string currentUser)
        public Guid CompleteWorkflowInstanceAction(WorkflowInstanceActionHistory workflowInstanceActionHistory, string currentUser)
        {
            Guid newAction = new Guid();
            //WorkflowStepOption opt = new WorkflowStepOption();
            //opt = opt.GetWorkflowStepOption(OptionID);
            //int numRequired = opt.NumberRequired;

            var optionId = new Guid(workflowInstanceActionHistory.OptionID.ToString());

            var workflowStepOption = _ssmMWorkFlowStepOption.Get(optionId);
            var required = workflowStepOption.Result.NumberRequired;

            WorkflowInstanceActionHistory action = new WorkflowInstanceActionHistory
            {
                WorkflowInstanceID = workflowInstanceActionHistory.WorkflowInstanceID,
                WorkflowStepID = workflowInstanceActionHistory.WorkflowStepID,
                OptionID = workflowInstanceActionHistory.OptionID,
                Action = workflowInstanceActionHistory.Action
            };

            //var instanceId = new Guid(workflowInstanceActionHistory.WorkflowInstanceID.ToString());
            //var completed = _ssmMWorkFlowStepOption.GetAllByInstanceId(instanceId);

            //    action.GetWorkflowInstanceActionHistoryByWorkflowInstanceID(InstanceID).Where(a => a.OptionID == OptionID).Count();
            //opt = opt.GetWorkflowStepOption(OptionID);
            //int numRequired = opt.NumberRequired;
            //WorkflowInstanceActionHistory action = new WorkflowInstanceActionHistory
            //{
            //    WorkflowInstanceID = InstanceID,
            //    WorkflowStepID = WorkflowStepID,
            //    OptionID = OptionID,
            //    Action = Action
            //};
            //int numDone = action.GetWorkflowInstanceActionHistoryByWorkflowInstanceID(InstanceID).Where(a => a.OptionID == OptionID).Count();
            //newAction = action.Save(currentUser);
            //if (numDone + 1 >= numRequired)
            //{
            //    action.Action = "CompleteStep";
            //    action.Save(currentUser);
            //    WorkflowInstance i = (new WorkflowInstance()).GetWorkflowInstance(InstanceID);
            //    if ((opt.NextStepID == Guid.Empty || opt.NextStepID == null) && opt.IsComplete)
            //    {
            //        i.CurrentWorkflowState = "Complete";
            //        i.CurrentWorkflowStepID = Guid.Empty;
            //    }
            //    else if ((opt.NextStepID == Guid.Empty || opt.NextStepID == null) && opt.IsTerminate)
            //    {
            //        i.CurrentWorkflowState = "Cancelled";
            //        i.CurrentWorkflowStepID = Guid.Empty;
            //    }
            //    else
            //    {
            //        i.CurrentWorkflowStepID = opt.NextStepID;
            //        i.CurrentWorkflowState = "InProcess";
            //    }
            //    i.Save(currentUser);
            //}
            return newAction;
        }
    }
}