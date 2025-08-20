//DashboardController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using WorkflowDocMgmt.Web.Models;
using TaskModel = WorkflowDocMgmt.Web.Models.Task;

namespace WorkflowDocMgmt.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IConfiguration _config;
        public DashboardController(IConfiguration config)
        {
            _config = config;
        }

        // GET: Dashboard
        public IActionResult Index()
        {
            int adminId = Convert.ToInt32(HttpContext.Session.GetString("AdminId"));
            List<TaskModel> tasks = new List<TaskModel>();

            using (var con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                using var cmd = new SqlCommand("SELECT * FROM Tasks WHERE AssignedAdminId=@AdminId AND Status='Pending'", con);
                cmd.Parameters.AddWithValue("@AdminId", adminId);
                con.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    tasks.Add(new TaskModel
                    {
                        TaskId = (int)reader["TaskId"],
                        DocumentId = (int)reader["DocumentId"],
                        WorkflowId = (int)reader["WorkflowId"],
                        AssignedAdminId = (int)reader["AssignedAdminId"],
                        Status = reader["Status"].ToString(),
                        StepNumber = (int)reader["StepNumber"],
                        CreatedAt = (DateTime)reader["CreatedAt"]
                    });
                }
            }
            return View(tasks);
        }

        // POST: Approve/Reject task
        [HttpPost]
        public IActionResult UpdateTaskStatus(int taskId, string action)
        {
            int adminId = Convert.ToInt32(HttpContext.Session.GetString("AdminId"));

            using (var con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                using var cmd = new SqlCommand("sp_UpdateTaskStatus", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TaskId", taskId);
                cmd.Parameters.AddWithValue("@Action", action); // "Approved" or "Rejected"
                cmd.Parameters.AddWithValue("@AdminId", adminId);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            TempData["Message"] = $"Task {taskId} {action} successfully!";
            return RedirectToAction("Index");
        }
    }
}
