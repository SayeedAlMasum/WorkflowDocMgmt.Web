//RegisterController.cs
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using WorkflowDocMgmt.Web.Models.ViewModels;

namespace WorkflowDocMgmt.Web.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IConfiguration _config;
        private readonly string _connectionString;

        public RegisterController(IConfiguration config)
        {
            _config = config;
            _connectionString = _config.GetConnectionString("DefaultConnection");
        }

        // GET: Register Page
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // POST: Register New Admin
        [HttpPost]
        public IActionResult Index(AdminRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegisterAdmin", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Username", model.Username);
                    cmd.Parameters.AddWithValue("@Password", model.Password);
                    cmd.Parameters.AddWithValue("@AccessLevel", model.AccessLevel);

                    con.Open();
                    try
                    {
                        cmd.ExecuteNonQuery();
                        ViewBag.Message = "Registration successful! Please login.";
                        return RedirectToAction("Index", "Login");
                    }
                    catch (SqlException ex)
                    {
                        ModelState.AddModelError("", ex.Message);
                    }
                }
            }
            return View(model);
        }
    }
}
