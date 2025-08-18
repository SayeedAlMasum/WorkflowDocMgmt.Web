//AdminRepository.cs
using System.Data;
using Microsoft.Data.SqlClient;
using WorkflowDocMgmt.Web.Models;

namespace WorkflowDocMgmt.Web.Data
{
    public class AdminRepository
    {
        private readonly Db _db;

        public AdminRepository(Db db) => _db = db;

        public DataTable GetAllAdmins() => _db.ExecuteSelect("sp_GetAllAdmins");

        public int CreateAdmin(Admin admin)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@UserName", admin.UserName),
                new SqlParameter("@Password", admin.Password),
                new SqlParameter("@AccessLevel", admin.AccessLevel.ToString()),
                new SqlParameter("@CreatedBy", admin.CreatedBy)
            };
            return _db.ExecuteDml("sp_CreateAdmin", parameters);
        }
    }
}
