//WorkflowRepository.cs
using WorkflowDocMgmt.Web.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace WorkflowDocMgmt.Web.Data
{
    public class WorkflowRepository
    {
        private readonly Db _db;

        public WorkflowRepository(Db db) => _db = db;

        public DataTable GetWorkflows() => _db.ExecuteSelect("sp_GetWorkflows");

        public int CreateWorkflow(Workflow workflow)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@Name", workflow.Name),
                new SqlParameter("@Type", workflow.Type.ToString()),
                new SqlParameter("@AssignedAdmins", string.Join(",", workflow.AssignedAdminIds)),
                new SqlParameter("@CreatedBy", workflow.CreatedBy)
            };
            return _db.ExecuteDml("sp_CreateWorkflow", parameters);
        }
    }
}
