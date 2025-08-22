using PMSCH.Server.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

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

        public string AssignTechnician(TechnicianMachineAssignment assignment)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Check if machine is already assigned
                string checkQuery = @"SELECT COUNT(*) FROM TechnicianMachineAssignments 
                                      WHERE MachineId = @MachineId";
                SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@MachineId", assignment.MachineId);

                int count = (int)checkCmd.ExecuteScalar();
                if (count > 0)
                {
                    return "Machine already assigned";
                }

                // Assign technician
                string insertQuery = @"INSERT INTO TechnicianMachineAssignments 
                                       (UserId, MachineId) 
                                       VALUES (@UserId, @MachineId)";
                SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                insertCmd.Parameters.AddWithValue("@UserId", assignment.UserId);
                insertCmd.Parameters.AddWithValue("@MachineId", assignment.MachineId);
                insertCmd.ExecuteNonQuery();

                return "Technician assigned successfully";
            } // ✅ This closing brace was missing
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

        public List<dynamic> GetDetailedAssignmentsByUsername(string username)
        {
            var assignments = new List<dynamic>();

            using (var conn = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT
                        m.MachineID,
                        m.Name AS MachineName,
                        mc.CategoryName,
                        mt.TypeName,
                        m.InstallationDate,
                        m.Status,
                        m.LifeCycle
                    FROM TechnicianMachineAssignments tma
                    JOIN Machines m ON tma.MachineId = m.MachineID
                    JOIN MachineCategories mc ON m.CategoryID = mc.CategoryID
                    JOIN MachineTypes mt ON m.TypeID = mt.TypeID
                    JOIN Users u ON tma.UserId = u.Id
                    WHERE u.Role = 'Technician' AND u.Username = @Username";

                var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);

                conn.Open();
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    assignments.Add(new
                    {
                        MachineID = Convert.ToInt32(reader["MachineID"]),
                        MachineName = reader["MachineName"].ToString(),
                        CategoryName = reader["CategoryName"].ToString(),
                        TypeName = reader["TypeName"].ToString(),
                        InstallationDate = Convert.ToDateTime(reader["InstallationDate"]),
                        Status = reader["Status"].ToString(),
                        LifeCycle = Convert.ToInt32(reader["LifeCycle"])
                    });
                }

                reader.Close();
            }

            return assignments;
        }
    }
}
