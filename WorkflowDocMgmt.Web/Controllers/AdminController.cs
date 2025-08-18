//AdminController.cs
using Microsoft.AspNetCore.Mvc;
using WorkflowDocMgmt.Web.Data;
using WorkflowDocMgmt.Web.Models;

namespace WorkflowDocMgmt.Web.Controllers
{
    public class AdminsController : Controller
    {
        private readonly AdminRepository _repo;

        public AdminsController(AdminRepository repo) => _repo = repo;

        public IActionResult Index()
        {
            var admins = _repo.GetAllAdmins();
            return View(admins);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Admin admin)
        {
            admin.CreatedBy = 1; // example: current logged-in admin
            _repo.CreateAdmin(admin);
            return RedirectToAction("Index");
        }
    }
}
