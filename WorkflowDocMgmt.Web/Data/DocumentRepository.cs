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

        // Get all documents
        public DataTable GetDocuments() => _db.ExecuteSelect("sp_GetAllDocuments");

        // Add new document
        public int AddDocument(Document doc)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@Title", doc.Title),
                new SqlParameter("@DocumentTypeId", doc.DocumentTypeId),
                new SqlParameter("@FilePath", doc.FilePath),
                new SqlParameter("@WorkflowId", doc.WorkflowId),
                new SqlParameter("@CreatedBy", doc.CreatedBy),
                new SqlParameter("@Status", doc.Status ?? "Pending"),
                new SqlParameter("@CurrentStageId", doc.CurrentStageId)
            };
            return _db.ExecuteDml("sp_AddDocument", parameters);
        }

        // Update document status or stage
        public int UpdateDocument(Document doc)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@DocumentId", doc.DocumentId),
                new SqlParameter("@Status", doc.Status),
                new SqlParameter("@CurrentStageId", doc.CurrentStageId)
            };
            return _db.ExecuteDml("sp_UpdateDocument", parameters);
        }

        // Delete document
        public int DeleteDocument(int id)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@DocumentId", id)
            };
            return _db.ExecuteDml("sp_DeleteDocument", parameters);
        }
    }
}
