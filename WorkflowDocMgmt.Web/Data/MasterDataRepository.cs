//MasterDataRepository.cs
using WorkflowDocMgmt.Web.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace WorkflowDocMgmt.Web.Data
{
    public class MasterDataRepository
    {
        private readonly Db _db;

        public MasterDataRepository(Db db) => _db = db;

        public DataTable GetDocumentTypes() => _db.ExecuteSelect("sp_GetDocumentTypes");

        public int CreateDocumentType(DocumentType docType)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@Name", docType.Name),
                new SqlParameter("@Description", docType.Description)
            };
            return _db.ExecuteDml("sp_CreateDocumentType", parameters);
        }

        public int UpdateDocumentType(DocumentType docType)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@DocumentTypeId", docType.DocumentTypeId),
                new SqlParameter("@Name", docType.Name),
                new SqlParameter("@Description", docType.Description)
            };
            return _db.ExecuteDml("sp_UpdateDocumentType", parameters);
        }

        public int DeleteDocumentType(int id)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@DocumentTypeId", id)
            };
            return _db.ExecuteDml("sp_DeleteDocumentType", parameters);
        }
    }
}
