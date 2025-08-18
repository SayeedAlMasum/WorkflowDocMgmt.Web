//DocumentRepository.cs
using WorkflowDocMgmt.Web.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace WorkflowDocMgmt.Web.Data
{
    public class DocumentRepository
    {
        private readonly Db _db;

        public DocumentRepository(Db db) => _db = db;

        public DataTable GetDocuments() => _db.ExecuteSelect("sp_GetDocuments");

        public int AddDocument(Document doc)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@Name", doc.Name),
                new SqlParameter("@DocumentTypeId", doc.DocumentTypeId),
                new SqlParameter("@FilePath", doc.FilePath),
                new SqlParameter("@WorkflowId", doc.WorkflowId),
                new SqlParameter("@CreatedBy", doc.CreatedBy)
            };
            return _db.ExecuteDml("sp_AddDocument", parameters);
        }
    }
}
