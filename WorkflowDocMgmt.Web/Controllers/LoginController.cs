using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using WorkflowDocMgmt.Web.Models.ViewModels;


namespace WorkflowDocMgmt.Web.Controllers
{
    public class LoginController : Controller
    {
            private readonly IConfiguration _config;
            private readonly string _connectionString;

            public LoginController(IConfiguration config)
            {
                _config = config;
                _connectionString = _config.GetConnectionString("DefaultConnection");
            }

            [HttpGet]
            public IActionResult Index()
            {
                return View();
            }

            [HttpPost]
            public IActionResult Index(AdminLoginViewModel model)
            {
                if (ModelState.IsValid)
                {
                    using (SqlConnection con = new SqlConnection(_connectionString))
                    {
                        SqlCommand cmd = new SqlCommand("sp_ValidateAdmin", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Username", model.Username);
                        cmd.Parameters.AddWithValue("@Password", model.Password);

                        con.Open();
                        SqlDataReader rdr = cmd.ExecuteReader();
                        if (rdr.Read())
                        {
                            HttpContext.Session.SetString("Username", rdr["Username"].ToString());
                            HttpContext.Session.SetString("AccessLevel", rdr["AccessLevel"].ToString());
                            return RedirectToAction("Index", "Dashboard");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Invalid login attempt.");
                        }
                    }
                }
                return View(model);
            }

            public IActionResult Logout()
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Login");
            }
        }
    }