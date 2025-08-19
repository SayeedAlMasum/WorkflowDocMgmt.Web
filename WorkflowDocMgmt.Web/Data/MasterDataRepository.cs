//MasterDataRepository.cs
using System.Data;
using Microsoft.Data.SqlClient;
using WorkflowDocMgmt.Web.Models;

namespace WorkflowDocMgmt.Web.Data
{
    public class MasterDataRepository
    {
        private readonly IConfiguration _config;

        public MasterDataRepository(IConfiguration config)
        {
            _config = config;
        }

        // Get all document types
        public DataTable GetDocumentTypes()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                SqlCommand cmd = new SqlCommand("sp_GetDocumentTypes", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        // Create new document type
        public void CreateDocumentType(DocumentType docType)
        {
            using (SqlConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                SqlCommand cmd = new SqlCommand("sp_CreateDocumentType", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", docType.Name);
                cmd.Parameters.AddWithValue("@Description", docType.Description ?? (object)DBNull.Value);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
