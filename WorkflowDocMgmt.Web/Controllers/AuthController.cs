//AuthController.cs
using Microsoft.AspNetCore.Mvc;
using WorkflowDocMgmt.Web.Data;
using WorkflowDocMgmt.Web.Models;

namespace WorkflowDocMgmt.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly AdminRepository _repo;

        public AuthController(IConfiguration config)
        {
            _repo = new AdminRepository(config);
        }

        // GET: Register
        [HttpGet]
        public IActionResult Register()
        {
            // যদি কোনো admin না থাকে → allow
            // অথবা যদি logged in Admin + ReadWrite থাকে → allow
            if (!_repo.AnyAdminExists() || HttpContext.Session.GetString("AccessLevel") == "ReadWrite")
            {
                return View();
            }

            TempData["Error"] = "You do not have permission to register new admins.";
            return RedirectToAction("Login");
        }

        [HttpPost]
        public IActionResult Register(string username, string password, string accessLevel)
        {
            if (!_repo.AnyAdminExists() || HttpContext.Session.GetString("AccessLevel") == "ReadWrite")
            {
                _repo.CreateAdmin(new Admin
                {
                    Username = username,
                    Password = password,
                    AccessLevel = accessLevel
                });

                TempData["Message"] = "✅ Admin registered successfully!";
                return RedirectToAction("Login");
            }

            TempData["Error"] = "Registration not allowed!";
            return RedirectToAction("Login");
        }

        // GET: Login
        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var admin = _repo.ValidateAdmin(username, password);
            if (admin != null)
            {
                HttpContext.Session.SetString("AdminId", admin.AdminId.ToString());
                HttpContext.Session.SetString("AccessLevel", admin.AccessLevel);
                HttpContext.Session.SetString("Username", admin.Username);

                return RedirectToAction("Dashboard");
            }

            ViewBag.Error = "Invalid username or password!";
            return View();
        }

        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("AdminId") == null)
                return RedirectToAction("Login");

            ViewBag.Admins = _repo.GetAllAdmins();
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
