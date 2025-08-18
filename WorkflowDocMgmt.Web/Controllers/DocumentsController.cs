//DocumentsController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using WorkflowDocMgmt.Web.Models;

public class DocumentController : Controller
{
    private readonly IConfiguration _config;
    private readonly IWebHostEnvironment _env;

    public DocumentController(IConfiguration config, IWebHostEnvironment env)
    {
        _config = config;
        _env = env;
    }

    // List all documents
    public IActionResult Index()
    {
        List<Document> documents = new List<Document>();
        using (SqlConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
        {
            SqlCommand cmd = new SqlCommand("sp_GetAllDocuments", con);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                documents.Add(new Document
                {
                    DocumentId = Convert.ToInt32(reader["DocumentId"]),
                    Title = reader["Title"].ToString(),
                    FilePath = reader["FilePath"].ToString(),
                    UploadedBy = reader["UploadedBy"].ToString(),
                    Status = reader["Status"].ToString(),
                    CurrentStageName = reader["StageName"]?.ToString(),
                    UploadedOn = Convert.ToDateTime(reader["UploadedOn"])
                });
            }
        }
        return View(documents);
    }

    // Upload - GET
    public IActionResult Upload()
    {
        List<WorkflowStage> stages = new List<WorkflowStage>();
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
        ViewBag.Stages = stages;
        return View();
    }

    // Upload - POST
    [HttpPost]
    public IActionResult Upload(string title, IFormFile file, int stageId)
    {
        if (file != null && file.Length > 0)
        {
            string uploads = Path.Combine(_env.WebRootPath, "documents");
            Directory.CreateDirectory(uploads);
            string filePath = Path.Combine(uploads, file.FileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            using (SqlConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                SqlCommand cmd = new SqlCommand("sp_InsertDocument", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Title", title);
                cmd.Parameters.AddWithValue("@FilePath", "/documents/" + file.FileName);
                cmd.Parameters.AddWithValue("@UploadedBy", User.Identity.Name ?? "Anonymous");
                cmd.Parameters.AddWithValue("@CurrentStageId", stageId);
                con.Open();
                cmd.ExecuteNonQuery();
            }

            TempData["Message"] = "Document uploaded successfully!";
        }
        return RedirectToAction("Index");
    }

    // Update Status (Approve/Reject & Move Next Stage)
    public IActionResult UpdateStatus(int id, string status, int? nextStageId)
    {
        using (SqlConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
        {
            SqlCommand cmd = new SqlCommand("sp_UpdateDocumentStatus", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@DocumentId", id);
            cmd.Parameters.AddWithValue("@Status", status);
            cmd.Parameters.AddWithValue("@NextStageId", nextStageId.HasValue ? nextStageId.Value : (object)DBNull.Value);
            con.Open();
            cmd.ExecuteNonQuery();
        }
        return RedirectToAction("Index");
    }
}
