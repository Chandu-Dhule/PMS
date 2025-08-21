using PMSCH.Server.Models;
using Microsoft.Data.SqlClient;

namespace PMSCH.Server.Repositories
{
    public class AnalysisRepository : IAnalysisRepository
    {
        private readonly string _connectionString;

        public AnalysisRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public AnalysisData GetAnalysisData(int userId, string role)
        {
            var result = new AnalysisData
            {
                MachineHealthData = new List<MachineHealthSummary>(),
                MaintenanceStatusData = new List<StatusCount>(),
                CategoryDistributionData = new List<CategoryCount>()
            };

            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            string machineFilterQuery = role switch
            {
                "Admin" => "",
                "Manager" => "WHERE m.CategoryID = (SELECT CategoryID FROM Users WHERE Id = @UserId)",
                "Technician" => "WHERE m.MachineID IN (SELECT MachineId FROM TechnicianMachineAssignments WHERE UserId = @UserId)",
                _ => throw new UnauthorizedAccessException("Invalid role")
            };

            // 1. Machine Health Data
            var healthCmd = new SqlCommand($@"
                SELECT m.Name, hm.HealthStatus, hm.Temperature
                FROM HealthMetrics hm
                INNER JOIN Machines m ON hm.MachineID = m.MachineID
                {machineFilterQuery}", conn);
            healthCmd.Parameters.AddWithValue("@UserId", userId);
            using var healthReader = healthCmd.ExecuteReader();
            while (healthReader.Read())
            {
                result.MachineHealthData.Add(new MachineHealthSummary
                {
                    Name = healthReader["Name"].ToString(),
                    Status = healthReader["HealthStatus"].ToString(),
                    Temp = Convert.ToSingle(healthReader["Temperature"])
                });
            }
            healthReader.Close();

            // 2. Maintenance Status Count
            var statusCmd = new SqlCommand($@"
                SELECT hm.HealthStatus, COUNT(*) AS Count
                FROM HealthMetrics hm
                INNER JOIN Machines m ON hm.MachineID = m.MachineID
                {machineFilterQuery}
                GROUP BY hm.HealthStatus", conn);
            statusCmd.Parameters.AddWithValue("@UserId", userId);
            using var statusReader = statusCmd.ExecuteReader();
            while (statusReader.Read())
            {
                result.MaintenanceStatusData.Add(new StatusCount
                {
                    Status = statusReader["HealthStatus"].ToString(),
                    Count = Convert.ToInt32(statusReader["Count"])
                });
            }
            statusReader.Close();

            // 3. Category Distribution
            var categoryCmd = new SqlCommand($@"
                SELECT mc.CategoryName, COUNT(*) AS Count
                FROM Machines m
                INNER JOIN MachineCategories mc ON m.CategoryID = mc.CategoryID
                {machineFilterQuery}
                GROUP BY mc.CategoryName", conn);
            categoryCmd.Parameters.AddWithValue("@UserId", userId);
            using var categoryReader = categoryCmd.ExecuteReader();
            while (categoryReader.Read())
            {
                result.CategoryDistributionData.Add(new CategoryCount
                {
                    Category = categoryReader["CategoryName"].ToString(),
                    Count = Convert.ToInt32(categoryReader["Count"])
                });
            }

            return result;
        }
    }
}