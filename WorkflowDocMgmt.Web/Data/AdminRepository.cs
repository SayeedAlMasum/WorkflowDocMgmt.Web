//AdminRepository.cs
using Microsoft.Data.SqlClient;
using System.Data;
using WorkflowDocMgmt.Web.Models;

namespace WorkflowDocMgmt.Web.Data
{
    public class AdminRepository
    {
        private readonly string _connectionString;

        public AdminRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public Admin? ValidateAdmin(string username, string password)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("SELECT TOP 1 * FROM Admins WHERE Username=@u AND Password=@p", conn);
            cmd.Parameters.AddWithValue("@u", username);
            cmd.Parameters.AddWithValue("@p", password);
            conn.Open();

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Admin
                {
                    AdminId = (int)reader["AdminId"],
                    Username = reader["Username"].ToString(),
                    AccessLevel = reader["AccessLevel"].ToString()
                };
            }
            return null;
        }

        public bool AnyAdminExists()
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("SELECT COUNT(*) FROM Admins", conn);
            conn.Open();
            return (int)cmd.ExecuteScalar() > 0;
        }

        public List<Admin> GetAllAdmins()
        {
            var list = new List<Admin>();
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("SELECT * FROM Admins ORDER BY CreatedAt DESC", conn);
            conn.Open();

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Admin
                {
                    AdminId = (int)reader["AdminId"],
                    Username = reader["Username"].ToString(),
                    AccessLevel = reader["AccessLevel"].ToString(),
                    CreatedAt = (DateTime)reader["CreatedAt"]
                });
            }
            return list;
        }

        public void CreateAdmin(Admin admin)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(
                "INSERT INTO Admins (Username,Password,AccessLevel,CreatedAt) VALUES (@u,@p,@a,GETDATE())", conn);
            cmd.Parameters.AddWithValue("@u", admin.Username);
            cmd.Parameters.AddWithValue("@p", admin.Password);
            cmd.Parameters.AddWithValue("@a", admin.AccessLevel);
            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
