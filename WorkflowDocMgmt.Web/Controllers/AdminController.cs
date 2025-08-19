//AdminController.cs
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using WorkflowDocMgmt.Web.Data;
using WorkflowDocMgmt.Web.Filters;
using WorkflowDocMgmt.Web.Models;
using WorkflowDocMgmt.Web.Models.ViewModels;

namespace WorkflowDocMgmt.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IConfiguration _config;
        public AdminController(IConfiguration config)
        {
            _config = config;
        }

        // GET: Registration Form (Only ReadWrite Admins can access)
        public IActionResult Register()
        {
            string accessLevel = HttpContext.Session.GetString("AccessLevel") ?? "";
            if (accessLevel != "ReadWrite")
            {
                TempData["Error"] = "You do not have permission to add new admins.";
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: Register new Admin
        [HttpPost]
        public IActionResult Register(AdminRegisterViewModel model)
        {
            int requestingAdminId = Convert.ToInt32(HttpContext.Session.GetString("AdminId"));

            using (SqlConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                SqlCommand cmd = new SqlCommand("sp_RegisterAdmin", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RequestingAdminId", requestingAdminId);
                cmd.Parameters.AddWithValue("@Username", model.Username);
                cmd.Parameters.AddWithValue("@Password", model.Password);
                cmd.Parameters.AddWithValue("@AccessLevel", model.AccessLevel);

                con.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                    TempData["Message"] = "✅ Admin created successfully!";
                    return RedirectToAction("AdminList");
                }
                catch (SqlException ex)
                {
                    TempData["Error"] = ex.Message;
                }
            }
            return View(model);
        }

        // List of Admins
        public IActionResult AdminList()
        {
            List<AdminRegisterViewModel> admins = new List<AdminRegisterViewModel>();
            using (SqlConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                SqlCommand cmd = new SqlCommand("SELECT Username, AccessLevel FROM Admins", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    admins.Add(new AdminRegisterViewModel
                    {
                        Username = reader["Username"].ToString(),
                        AccessLevel = reader["AccessLevel"].ToString()
                    });
                }
            }
            return View(admins);
        }
    }
}