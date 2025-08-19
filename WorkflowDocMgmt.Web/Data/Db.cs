//Db.cs
// Db.cs
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace WorkflowDocMgmt.Web.Data
{
    public class Db
    {
        private readonly string _connectionString;
        public Db(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public DataTable ExecuteSelect(string spName, SqlParameter[] parameters = null)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(spName, con);
            cmd.CommandType = CommandType.StoredProcedure;
            if (parameters != null) cmd.Parameters.AddRange(parameters);
            using var adapter = new SqlDataAdapter(cmd);
            var dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        public int ExecuteDml(string spName, SqlParameter[] parameters)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(spName, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddRange(parameters);
            con.Open();
            return cmd.ExecuteNonQuery();
        }

        public int ExecuteDmlScalar(string spName, SqlParameter[] parameters)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(spName, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddRange(parameters);
            con.Open();
            return Convert.ToInt32(cmd.ExecuteScalar());
        }
    }
}
