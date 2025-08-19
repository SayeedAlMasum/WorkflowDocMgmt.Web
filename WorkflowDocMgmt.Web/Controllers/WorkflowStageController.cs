//WorkflowStageController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using WorkflowDocMgmt.Web.Models;

public class WorkflowStageController : Controller
{
    private readonly IConfiguration _config;

    public WorkflowStageController(IConfiguration config)
    {
        _config = config;
    }

    // List all stages
    public IActionResult Index()
    {
        List<WorkflowStage> stages = new();
        using (SqlConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
        {
            SqlCommand cmd = new SqlCommand("sp_GetWorkflowStages", con);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                stages.Add(new WorkflowStage
                {
                    StageId = Convert.ToInt32(reader["StageId"]),
                    StageName = reader["StageName"].ToString()
                });
            }
        }
        return View(stages);
    }

    // Add stage - GET
    public IActionResult Create() => View();

    // Add stage - POST
    [HttpPost]
    public IActionResult Create(string stageName)
    {
        using (SqlConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
        {
            SqlCommand cmd = new SqlCommand("sp_AddWorkflowStage", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@StageName", stageName);
            con.Open();
            cmd.ExecuteNonQuery();
        }
        return RedirectToAction("Index");
    }

    // Edit stage - GET
    public IActionResult Edit(int id)
    {
        WorkflowStage stage = new();
        using (SqlConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM WorkflowStages WHERE StageId=@id", con);
            cmd.Parameters.AddWithValue("@id", id);
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                stage.StageId = Convert.ToInt32(reader["StageId"]);
                stage.StageName = reader["StageName"].ToString();
            }
        }
        return View(stage);
    }

    // Edit stage - POST
    [HttpPost]
    public IActionResult Edit(WorkflowStage stage)
    {
        using (SqlConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
        {
            SqlCommand cmd = new SqlCommand("sp_UpdateWorkflowStage", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@StageId", stage.StageId);
            cmd.Parameters.AddWithValue("@StageName", stage.StageName);
            con.Open();
            cmd.ExecuteNonQuery();
        }
        return RedirectToAction("Index");
    }

    // Delete stage
    public IActionResult Delete(int id)
    {
        using (SqlConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
        {
            SqlCommand cmd = new SqlCommand("sp_DeleteWorkflowStage", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@StageId", id);
            con.Open();
            cmd.ExecuteNonQuery();
        }
        return RedirectToAction("Index");
    }
}
