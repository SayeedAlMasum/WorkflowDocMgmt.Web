//WorkflowRepository.cs
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using WorkflowDocMgmt.Web.Models;

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
                new SqlParameter("@CreatedBy", workflow.CreatedBy)
            };

            int workflowId = _db.ExecuteDmlScalar("sp_CreateWorkflow", parameters);

            // Assign admins to workflow
            if (workflow.AssignedAdminIds != null)
            {
                foreach (var adminId in workflow.AssignedAdminIds)
                {
                    var assignParams = new SqlParameter[]
                    {
                        new SqlParameter("@WorkflowId", workflowId),
                        new SqlParameter("@AdminId", adminId)
                    };
                    _db.ExecuteDml("sp_AssignAdminToWorkflow", assignParams);
                }
            }

            return workflowId;
        }
    }
}
