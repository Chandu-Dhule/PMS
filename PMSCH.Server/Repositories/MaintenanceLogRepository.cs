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
            INNER JOIN Logins u ON m.CategoryID = u.CategoryID
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
                conn.Open();

                // Insert maintenance log
                string insertLogQuery = @"INSERT INTO MaintenanceLogs 
                                  (LogID,MachineID, MaintenanceDate, Description, OperatorName, NextDueDate) 
                                  VALUES 
                                  (@LogID,@MachineID, @MaintenanceDate, @Description, @OperatorName, @NextDueDate)";
                SqlCommand logCmd = new SqlCommand(insertLogQuery, conn);
                logCmd.Parameters.AddWithValue("@LogID", log.LogID);
                logCmd.Parameters.AddWithValue("@MachineID", log.MachineID);
                logCmd.Parameters.AddWithValue("@MaintenanceDate", log.MaintenanceDate);
                logCmd.Parameters.AddWithValue("@Description", log.Description);
                logCmd.Parameters.AddWithValue("@OperatorName", log.OperatorName);
                logCmd.Parameters.AddWithValue("@NextDueDate", log.NextDueDate);
                logCmd.ExecuteNonQuery();

                // Update latest HealthMetric for this machine
                string updateMetricQuery = @"UPDATE HealthMetrics 
                                     SET Temperature = @Temperature, 
                                         EnergyConsumption = @EnergyConsumption, 
                                         HealthStatus = @HealthStatus, 
                                         CheckDate = @CheckDate
                                     WHERE MachineID = @MachineID 
                                     AND MetricID = (
                                         SELECT TOP 1 MetricID 
                                         FROM HealthMetrics 
                                         WHERE MachineID = @MachineID 
                                         ORDER BY CheckDate DESC
                                     )";
                SqlCommand metricCmd = new SqlCommand(updateMetricQuery, conn);
                metricCmd.Parameters.AddWithValue("@Temperature", log.Temperature);
                metricCmd.Parameters.AddWithValue("@EnergyConsumption", log.EnergyConsumption);
                metricCmd.Parameters.AddWithValue("@HealthStatus", log.HealthStatus);
                metricCmd.Parameters.AddWithValue("@CheckDate", DateTime.Now);
                metricCmd.Parameters.AddWithValue("@MachineID", log.MachineID);
                metricCmd.ExecuteNonQuery();
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
