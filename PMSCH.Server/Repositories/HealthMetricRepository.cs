using Microsoft.Extensions.Configuration;
using PMSCH.Server.Models;
using System;
using System.Collections.Generic;

using MySql.Data.MySqlClient;
namespace PMSCH.Server.Repositories { 

        public class HealthMetricRepository
        {
            private readonly string _connectionString;

            public HealthMetricRepository(IConfiguration config)
            {
                _connectionString = config.GetConnectionString("DefaultConnection");
            }

        public List<HealthMetric> GetAll()
        {
            var metrics = new List<HealthMetric>();

            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                string query = "SELECT * FROM HealthMetrics ";
                MySqlCommand cmd = new MySqlCommand(query, conn);

                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    metrics.Add(new HealthMetric
                    {
                        MetricID = Convert.ToInt32(reader["MetricID"]),
                        MachineID = Convert.ToInt32(reader["MachineID"]),
                        CheckDate = Convert.ToDateTime(reader["CheckDate"]),
                        Temperature = Convert.ToSingle(reader["Temperature"]),
                        EnergyConsumption = Convert.ToSingle(reader["EnergyConsumption"]),
                        HealthStatus = reader["HealthStatus"].ToString()
                    });
                }

                reader.Close();
            }

            return metrics;
        }

        // ✅ Get health metrics by machine ID
        public List<HealthMetric> GetByMachineId(int machineId)
            {
                var metrics = new List<HealthMetric>();

                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    string query = "SELECT * FROM HealthMetrics WHERE MachineID = @MachineID ";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MachineID", machineId);

                    conn.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        metrics.Add(new HealthMetric
                        {
                            MetricID = Convert.ToInt32(reader["MetricID"]),
                            MachineID = Convert.ToInt32(reader["MachineID"]),
                            CheckDate = Convert.ToDateTime(reader["CheckDate"]),
                            Temperature = Convert.ToSingle(reader["Temperature"]),
                            EnergyConsumption = Convert.ToSingle(reader["EnergyConsumption"]),
                           // Pressure = Convert.ToSingle(reader["Pressure"]),
                            HealthStatus = reader["HealthStatus"].ToString()
                        });
                    }

                    reader.Close();
                }

                return metrics;
            }

            //  Add a new health metric
            public void Add(HealthMetric metric)
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    string query = @"INSERT INTO HealthMetrics 
                                (MetricID,MachineID, CheckDate, Temperature, EnergyConsumption, HealthStatus) 
                                VALUES 
                                (@MetricID,@MachineID, @CheckDate, @Temperature, @EnergyConsumption, @HealthStatus)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MetricID", metric.MetricID);
                    cmd.Parameters.AddWithValue("@MachineID", metric.MachineID);
                    cmd.Parameters.AddWithValue("@CheckDate", metric.CheckDate);
                    cmd.Parameters.AddWithValue("@Temperature", metric.Temperature);
                   // cmd.Parameters.AddWithValue("@VibrationLevel", metric.VibrationLevel);
                    cmd.Parameters.AddWithValue("@EnergyConsumption", metric.EnergyConsumption);
                    cmd.Parameters.AddWithValue("@HealthStatus", metric.HealthStatus);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

        //  Get machines with critical health metrics (example: Temperature > threshold)
        public List<HealthMetric> GetCriticalMachines(float temperatureThreshold = 80.0f, float TEnergy=100.0f)
        {
            var machineDetails = new List<HealthMetric>();

            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                string query = "SELECT * FROM HealthMetrics WHERE Temperature > @Threshold or EnergyConsumption > @TEnergy";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Threshold", temperatureThreshold);
                cmd.Parameters.AddWithValue("@TEnergy", TEnergy);

                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    machineDetails.Add(new HealthMetric
                    {
                        MetricID = Convert.ToInt32(reader["MetricID"]),
                        MachineID = Convert.ToInt32(reader["MachineID"]),
                        CheckDate = Convert.ToDateTime(reader["CheckDate"]),
                        Temperature = Convert.ToSingle(reader["Temperature"]),
                        EnergyConsumption = Convert.ToSingle(reader["EnergyConsumption"]),
                        HealthStatus = reader["HealthStatus"].ToString()
                    });
                }
            }

            return machineDetails;
        }
        public List<HealthMetric> GetCriticalMachinesByTechnician(int technicianId, float temperatureThreshold = 80.0f, float TEnergy = 100.0f)
        {
            var machineDetails = new List<HealthMetric>();

            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                string query = @"
SELECT hm.* FROM HealthMetrics hm
INNER JOIN TechnicianMachineAssignments tma ON hm.MachineID = tma.MachineId
WHERE tma.UserId = @TechnicianId AND (hm.Temperature > @Threshold OR hm.EnergyConsumption > @TEnergy)";


                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TechnicianId", technicianId);
                cmd.Parameters.AddWithValue("@Threshold", temperatureThreshold);
                cmd.Parameters.AddWithValue("@TEnergy", TEnergy);

                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    machineDetails.Add(new HealthMetric
                    {
                        MetricID = Convert.ToInt32(reader["MetricID"]),
                        MachineID = Convert.ToInt32(reader["MachineID"]),
                        CheckDate = Convert.ToDateTime(reader["CheckDate"]),
                        Temperature = Convert.ToSingle(reader["Temperature"]),
                        EnergyConsumption = Convert.ToSingle(reader["EnergyConsumption"]),
                        HealthStatus = reader["HealthStatus"].ToString()
                    });
                }
            }

            return machineDetails;
        }
        public List<HealthMetric> GetCriticalMachinesByManager(int managerId, float temperatureThreshold = 80.0f, float TEnergy = 100.0f)
        {
            var machineDetails = new List<HealthMetric>();

            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                string query = @"
SELECT hm.* FROM HealthMetrics hm
INNER JOIN Machines m ON hm.MachineID = m.MachineID
INNER JOIN Logins u ON m.CategoryID = u.CategoryID
WHERE u.Id = @ManagerId AND u.Role = 'Manager' AND (hm.Temperature > @Threshold OR hm.EnergyConsumption > @TEnergy)";


                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ManagerId", managerId);
                cmd.Parameters.AddWithValue("@Threshold", temperatureThreshold);
                cmd.Parameters.AddWithValue("@TEnergy", TEnergy);

                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    machineDetails.Add(new HealthMetric
                    {
                        MetricID = Convert.ToInt32(reader["MetricID"]),
                        MachineID = Convert.ToInt32(reader["MachineID"]),
                        CheckDate = Convert.ToDateTime(reader["CheckDate"]),
                        Temperature = Convert.ToSingle(reader["Temperature"]),
                        EnergyConsumption = Convert.ToSingle(reader["EnergyConsumption"]),
                        HealthStatus = reader["HealthStatus"].ToString()
                    });
                }
            }

            return machineDetails;
        }


        public List<HealthMetric> GetHealthMetricsByTechnician(int technicianId)
        {
            var metrics = new List<HealthMetric>();

            using (var conn = new MySqlConnection(_connectionString))
            {
                string query = @"
            SELECT hm.* FROM HealthMetrics hm
            INNER JOIN TechnicianMachineAssignments tma ON hm.MachineID = tma.MachineId
            WHERE tma.UserId = @TechnicianId
            ORDER BY hm.CheckDate ";

                var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TechnicianId", technicianId);

                conn.Open();
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    metrics.Add(new HealthMetric
                    {
                        MetricID = Convert.ToInt32(reader["MetricID"]),
                        MachineID = Convert.ToInt32(reader["MachineID"]),
                        CheckDate = Convert.ToDateTime(reader["CheckDate"]),
                        Temperature = Convert.ToSingle(reader["Temperature"]),
                        EnergyConsumption = Convert.ToSingle(reader["EnergyConsumption"]),
                        HealthStatus = reader["HealthStatus"].ToString()
                    });
                }
            }

            return metrics;
        }
        public List<HealthMetric> GetHealthMetricsByManager(int managerId)
        {
            var metrics = new List<HealthMetric>();

            using (var conn = new MySqlConnection(_connectionString))
            {
                string query = @"
            SELECT hm.* FROM HealthMetrics hm
            INNER JOIN Machines m ON hm.MachineID = m.MachineID
            INNER JOIN Logins u ON m.CategoryID = u.CategoryID
            WHERE u.Id = @ManagerId AND u.Role = 'Manager'
            ORDER BY hm.CheckDate ";

                var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ManagerId", managerId);

                conn.Open();
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    metrics.Add(new HealthMetric
                    {
                        MetricID = Convert.ToInt32(reader["MetricID"]),
                        MachineID = Convert.ToInt32(reader["MachineID"]),
                        CheckDate = Convert.ToDateTime(reader["CheckDate"]),
                        Temperature = Convert.ToSingle(reader["Temperature"]),
                        EnergyConsumption = Convert.ToSingle(reader["EnergyConsumption"]),
                        HealthStatus = reader["HealthStatus"].ToString()
                    });
                }
            }

            return metrics;
        }

    }
}

