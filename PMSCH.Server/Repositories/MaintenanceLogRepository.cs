using Microsoft.Extensions.Configuration;
using PMSCH.Server.Models;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace PMSCH.Server.Repositories
{
    public class MaintenanceLogRepository
    {
        private readonly string _connectionString;

        public MaintenanceLogRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        // ✅ Get all logs
        public List<MaintenanceLog> GetAll()
        {
            var logs = new List<MaintenanceLog>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"SELECT LogID, MachineID, MaintenanceDate, Description, OperatorName, NextDueDate 
                                 FROM MaintenanceLogs 
                                 ORDER BY MaintenanceDate";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    logs.Add(new MaintenanceLog
                    {
                        LogID = Convert.ToInt32(reader["LogID"]),
                        MachineID = Convert.ToInt32(reader["MachineID"]),
                        MaintenanceDate = Convert.ToDateTime(reader["MaintenanceDate"]),
                        Description = reader["Description"].ToString() ?? string.Empty,
                        OperatorName = reader["OperatorName"].ToString() ?? string.Empty,
                        NextDueDate = Convert.ToDateTime(reader["NextDueDate"])
                    });
                }

                reader.Close();
            }

            return logs;
        }
        public List<MaintenanceLog> GetLogsByTechnician(int technicianId)
        {
            var logs = new List<MaintenanceLog>();
            using (var conn = new SqlConnection(_connectionString))
            {
                string query = @"
            SELECT ml.* FROM MaintenanceLogs ml
            INNER JOIN TechnicianMachineAssignments tma ON ml.MachineID = tma.MachineId
            WHERE tma.UserId = @TechnicianId
            ORDER BY ml.MaintenanceDate ASC";

                var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TechnicianId", technicianId);

                conn.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    logs.Add(new MaintenanceLog
                    {
                        LogID = Convert.ToInt32(reader["LogID"]),
                        MachineID = Convert.ToInt32(reader["MachineID"]),
                        MaintenanceDate = Convert.ToDateTime(reader["MaintenanceDate"]),
                        Description = reader["Description"].ToString(),
                        OperatorName = reader["OperatorName"].ToString(),
                        NextDueDate = Convert.ToDateTime(reader["NextDueDate"])
                    });
                }
            }
            return logs;
        }
        public List<MaintenanceLog> GetLogsByManager(int managerId)
        {
            var logs = new List<MaintenanceLog>();
            using (var conn = new SqlConnection(_connectionString))
            {
                string query = @"
            SELECT ml.* FROM MaintenanceLogs ml
            INNER JOIN Machines m ON ml.MachineID = m.MachineID
            INNER JOIN Users u ON m.CategoryID = u.CategoryID
            WHERE u.Id = @ManagerId AND u.Role = 'Manager'
            ORDER BY ml.MaintenanceDate";

                var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ManagerId", managerId);

                conn.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    logs.Add(new MaintenanceLog
                    {
                        LogID = Convert.ToInt32(reader["LogID"]),
                        MachineID = Convert.ToInt32(reader["MachineID"]),
                        MaintenanceDate = Convert.ToDateTime(reader["MaintenanceDate"]),
                        Description = reader["Description"].ToString(),
                        OperatorName = reader["OperatorName"].ToString(),
                        NextDueDate = Convert.ToDateTime(reader["NextDueDate"])
                    });
                }
            }
            return logs;
        }

        // ✅ Get logs by machine ID
        public List<MaintenanceLog> GetByMachineId(int machineId)
        {
            var logs = new List<MaintenanceLog>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"SELECT LogID, MachineID, MaintenanceDate, Description, OperatorName, NextDueDate 
                                 FROM MaintenanceLogs 
                                 WHERE MachineID = @MachineID 
                                 ORDER BY MaintenanceDate ";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MachineID", machineId);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    logs.Add(new MaintenanceLog
                    {
                        LogID = Convert.ToInt32(reader["LogID"]),
                        MachineID = Convert.ToInt32(reader["MachineID"]),
                        MaintenanceDate = Convert.ToDateTime(reader["MaintenanceDate"]),
                        Description = reader["Description"].ToString() ?? string.Empty,
                        OperatorName = reader["OperatorName"].ToString() ?? string.Empty,
                        NextDueDate = Convert.ToDateTime(reader["NextDueDate"])
                    });
                }

                reader.Close();
            }

            return logs;
        }

        // ✅ Add a new maintenance log
        public void Add(MaintenanceLog log)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO MaintenanceLogs 
                                (MachineID, MaintenanceDate, Description, OperatorName, NextDueDate) 
                                VALUES 
                                (@MachineID, @MaintenanceDate, @Description, @OperatorName, @NextDueDate)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MachineID", log.MachineID);
                cmd.Parameters.AddWithValue("@MaintenanceDate", log.MaintenanceDate);
                cmd.Parameters.AddWithValue("@Description", log.Description);
                cmd.Parameters.AddWithValue("@OperatorName", log.OperatorName);
                cmd.Parameters.AddWithValue("@NextDueDate", log.NextDueDate);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // ✅ Get machines due for maintenance
        public List<int> GetMachinesDue()
        {
            var dueMachines = new List<int>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"SELECT DISTINCT MachineID 
                                 FROM MaintenanceLogs 
                                 WHERE NextDueDate <= @Today";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Today", DateTime.Today);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    dueMachines.Add(Convert.ToInt32(reader["MachineID"]));
                }

                reader.Close();
            }

            return dueMachines;
        }
    }
}
