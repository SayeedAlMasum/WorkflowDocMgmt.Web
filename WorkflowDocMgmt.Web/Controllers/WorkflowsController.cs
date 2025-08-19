//WorkflowsController.cs
using Microsoft.AspNetCore.Mvc;
using WorkflowDocMgmt.Web.Data;
using WorkflowDocMgmt.Web.Models;
using System.Collections.Generic;
using System.Data;

namespace WorkflowDocMgmt.Web.Controllers
{
    public class WorkflowsController : Controller
    {
        private readonly WorkflowRepository _repo;
        private readonly AdminRepository _adminRepo;

        public WorkflowsController(WorkflowRepository repo, AdminRepository adminRepo)
        {
            _repo = repo;
            _adminRepo = adminRepo;
        }

        public IActionResult Index()
        {
            var workflows = _repo.GetWorkflows();
            return View(workflows);
        }

        public IActionResult Create()
        {
            var admins = _adminRepo.GetAllAdmins();
            ViewBag.Admins = admins;
            return View();
        }

        [HttpPost]
        public IActionResult Create(string name, WorkflowType type, List<int> assignedAdminIds)
        {
            var workflow = new Workflow
            {
                Name = name,
                Type = type,
                AssignedAdminIds = assignedAdminIds,
                CreatedBy = 1 // logged-in admin ID placeholder
            };
            _repo.CreateWorkflow(workflow);
            return RedirectToAction("Index");
        }
    }
}
