using PMSCH.Server.Models;
using Microsoft.Data.SqlClient;

namespace PMSCH.Server.Repositories
{
    public class TechnicianMachineAssignmentRepository
    {
        private readonly string _connectionString;

        public TechnicianMachineAssignmentRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<int> GetAssignedMachines(int userId)
        {
            var machineIds = new List<int>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT MachineId FROM TechnicianMachineAssignments WHERE UserId = @UserId", conn);
                cmd.Parameters.AddWithValue("@UserId", userId);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        machineIds.Add(reader.GetInt32(0));
                    }
                }
            }
            return machineIds;
        }

        public void AssignMachine(int userId, int machineId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("INSERT INTO TechnicianMachineAssignments (UserId, MachineId) VALUES (@UserId, @MachineId)", conn);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@MachineId", machineId);
                cmd.ExecuteNonQuery();
            }
        }

        public void RemoveAssignment(int userId, int machineId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("DELETE FROM TechnicianMachineAssignments WHERE UserId = @UserId AND MachineId = @MachineId", conn);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@MachineId", machineId);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
