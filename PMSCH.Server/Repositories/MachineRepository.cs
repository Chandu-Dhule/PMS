using Azure;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PMSCH.Server.Models;
using System;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PMSCH.Server.Repositories
{
    public class MachineRepository
    {
        private readonly string _connectionString;

        public MachineRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public List<Machine> GetAll()
        {
            var machines = new List<Machine>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
        SELECT m.MachineID, m.Name, m.CategoryID, m.TypeID, m.InstallationDate, m.Status,
       m.LifeCycle, hm.Temperature,hm.EnergyConsumption,l.[user] As AssignedTo
               FROM Machines m
             LEFT JOIN HealthMetrics hm ON m.MachineID = hm.MachineID
          LEFT JOIN TechnicianMachineAssignments t ON m.MachineID = t.MachineID
           LEFT JOIN Logins l ON t.UserId = l.id
		and l.Role='Technician'";



                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    machines.Add(new Machine
                    {
                        MachineID = Convert.ToInt32(reader["MachineID"]),
                        Name = reader["Name"].ToString(),
                        CategoryID = Convert.ToInt32(reader["CategoryID"]),
                        TypeID = Convert.ToInt32(reader["TypeID"]),
                        InstallationDate = Convert.ToDateTime(reader["InstallationDate"]),
                        Status = reader["Status"].ToString(),
                        LifeCycle = reader["LifeCycle"] != DBNull.Value ? Convert.ToInt32(reader["LifeCycle"]) : 0,
                        Temperature = reader["Temperature"] != DBNull.Value ? Convert.ToSingle(reader["Temperature"]) : (float?)null,
                        AssignedTo = reader["AssignedTo"].ToString(),
                        EnergyConsumption = reader["EnergyConsumption"] !=DBNull.Value ? Convert.ToInt32(reader["EnergyConsumption"]):(float?)null
                    });
                }
            }
            return machines;
        }

        public List<Machine> GetMachinesByTechnician(int technicianId)
        {
            var machines = new List<Machine>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @" SELECT m.MachineID, m.Name, m.CategoryID, m.TypeID, m.InstallationDate, m.Status,
       m.LifeCycle, hm.Temperature,hm.EnergyConsumption
FROM Machines m
LEFT JOIN HealthMetrics hm ON m.MachineID = hm.MachineID
LEFT JOIN TechnicianMachineAssignments t ON m.MachineID = t.MachineID
        
        INNER JOIN TechnicianMachineAssignments tma ON m.MachineID = tma.MachineId
        WHERE tma.UserId = @TechnicianId";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TechnicianId", technicianId);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    machines.Add(new Machine
                    {

                        MachineID = Convert.ToInt32(reader["MachineID"]),
                        Name = reader["Name"].ToString(),
                        CategoryID = Convert.ToInt32(reader["CategoryID"]),
                        TypeID = Convert.ToInt32(reader["TypeID"]),
                        InstallationDate = Convert.ToDateTime(reader["InstallationDate"]),
                        Status = reader["Status"].ToString(),
                        LifeCycle = reader["LifeCycle"] != DBNull.Value ? Convert.ToInt32(reader["LifeCycle"]) : 0,
                        Temperature = reader["Temperature"] != DBNull.Value ? Convert.ToSingle(reader["Temperature"]) : (float?)null,
                        //AssignedTo = reader["AssignedTo"].ToString(),
                        EnergyConsumption = reader["EnergyConsumption"] != DBNull.Value ? Convert.ToInt32(reader["EnergyConsumption"]) : (float?)null
                    });
                }
            }
            return machines;
        }


        public List<Machine> GetMachinesByManager(int managerId)
        {
            var machines = new List<Machine>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
        SELECT m.MachineID, m.Name, m.CategoryID, m.TypeID, m.InstallationDate, m.Status,
       m.LifeCycle, hm.Temperature,hm.EnergyConsumption,l.[user] As AssignedTo
FROM Machines m
LEFT JOIN HealthMetrics hm ON m.MachineID = hm.MachineID
LEFT JOIN TechnicianMachineAssignments t ON m.MachineID = t.MachineID
        LEFT JOIN Logins l ON t.UserId = l.id
		and l.Role='Technician'
        INNER JOIN Logins u ON m.CategoryID = u.CategoryID
        WHERE u.Id = @ManagerId AND u.Role = 'Manager'";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ManagerId", managerId);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    machines.Add(new Machine
                    {
                        MachineID = Convert.ToInt32(reader["MachineID"]),
                        Name = reader["Name"].ToString(),
                        CategoryID = Convert.ToInt32(reader["CategoryID"]),
                        TypeID = Convert.ToInt32(reader["TypeID"]),
                        InstallationDate = Convert.ToDateTime(reader["InstallationDate"]),
                        Status = reader["Status"].ToString(),
                        LifeCycle = reader["LifeCycle"] != DBNull.Value ? Convert.ToInt32(reader["LifeCycle"]):0,
                        Temperature = reader["Temperature"] != DBNull.Value ? Convert.ToSingle(reader["Temperature"]) : (float?)null
                    });
                }
            }
            return machines;
        }



        public Machine? GetById(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
            SELECT m.*, hm.Temperature, hm.EnergyConsumption, hm.HealthStatus
            FROM Machines m
            OUTER APPLY (
                SELECT TOP 1 Temperature, EnergyConsumption, HealthStatus
                FROM HealthMetrics
                WHERE MachineID = m.MachineID
                ORDER BY CheckDate DESC
            ) hm
            WHERE m.MachineID = @MachineID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MachineID", id);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new Machine
                    {
                        MachineID = Convert.ToInt32(reader["MachineID"]),
                        Name = reader["Name"].ToString(),
                        CategoryID = Convert.ToInt32(reader["CategoryID"]),
                        TypeID = Convert.ToInt32(reader["TypeID"]),
                        InstallationDate = Convert.ToDateTime(reader["InstallationDate"]),
                        Status = reader["Status"].ToString(),
                        Temperature = reader["Temperature"] != DBNull.Value ? Convert.ToInt32(reader["Temperature"]) : null,
                        EnergyConsumption = reader["EnergyConsumption"] != DBNull.Value ? Convert.ToInt32(reader["EnergyConsumption"]) : null,
                        HealthStatus = reader["HealthStatus"]?.ToString()
                    };
                }

                reader.Close();
            }

            return null;
        }




 
public bool Add(Machine machine)

{

    using (SqlConnection conn = new SqlConnection(_connectionString))

    {

        conn.Open();

        // Check if machine already exists

        string checkQuery = "SELECT COUNT(*) FROM Machines WHERE MachineID = @MachineID";

        SqlCommand checkCmd = new SqlCommand(checkQuery, conn);

        checkCmd.Parameters.AddWithValue("@MachineID", machine.MachineID);

        int count = (int)checkCmd.ExecuteScalar();

        if (count > 0)

        {

            // Machine already exists, skip insertion

            return false;

        }

        // Insert machine with LifeCycle

        string insertMachineQuery = @"INSERT INTO Machines
 
(MachineID, Name, CategoryID, TypeID, InstallationDate, Status, LifeCycle)
 
VALUES
 
(@MachineID, @Name, @CategoryID, @TypeID, @InstallationDate, @Status, @LifeCycle)";

        SqlCommand insertCmd = new SqlCommand(insertMachineQuery, conn);

        insertCmd.Parameters.AddWithValue("@MachineID", machine.MachineID);

        insertCmd.Parameters.AddWithValue("@Name", machine.Name);

        insertCmd.Parameters.AddWithValue("@CategoryID", machine.CategoryID);

        insertCmd.Parameters.AddWithValue("@TypeID", machine.TypeID);

        insertCmd.Parameters.AddWithValue("@InstallationDate", machine.InstallationDate);

        insertCmd.Parameters.AddWithValue("@Status", machine.Status);

        insertCmd.Parameters.AddWithValue("@LifeCycle", machine.LifeCycle); // ✅ Added

        insertCmd.ExecuteNonQuery();

        // Insert health metric

        string getMaxIdQuery = "SELECT ISNULL(MAX(MetricID), 0) + 1 FROM HealthMetrics";

        SqlCommand getIdCmd = new SqlCommand(getMaxIdQuery, conn);

        int newMetricId = (int)getIdCmd.ExecuteScalar();

        string insertMetricQuery = @"INSERT INTO HealthMetrics
 
(MetricID, MachineID, CheckDate, Temperature, EnergyConsumption, HealthStatus)
 
VALUES
 
(@MetricID, @MachineID, @CheckDate, @Temperature, @EnergyConsumption, @HealthStatus)";

        SqlCommand metricCmd = new SqlCommand(insertMetricQuery, conn);

        metricCmd.Parameters.AddWithValue("@MetricID", newMetricId);

        metricCmd.Parameters.AddWithValue("@MachineID", machine.MachineID);

        metricCmd.Parameters.AddWithValue("@CheckDate", DateTime.Now);

        metricCmd.Parameters.AddWithValue("@Temperature", machine.Temperature ?? 0);

        metricCmd.Parameters.AddWithValue("@EnergyConsumption", machine.EnergyConsumption);

        metricCmd.Parameters.AddWithValue("@HealthStatus", machine.HealthStatus);

        metricCmd.ExecuteNonQuery();

        return true;

    }

}








//        public bool Add(Machine machine)

//        {

//            using (SqlConnection conn = new SqlConnection(_connectionString))

//            {

//                conn.Open();

//                // Check if machine already exists

//                string checkQuery = "SELECT COUNT(*) FROM Machines WHERE MachineID = @MachineID";

//                SqlCommand checkCmd = new SqlCommand(checkQuery, conn);

//                checkCmd.Parameters.AddWithValue("@MachineID", machine.MachineID);

//                int count = (int)checkCmd.ExecuteScalar();

//                if (count > 0)

//                {

//                    // Machine already exists, skip insertion

//                    return false;

//                }

//                // Insert machine with LifeCycle

//                string insertMachineQuery = @"INSERT INTO Machines

//(MachineID, Name, CategoryID, TypeID, InstallationDate, Status, LifeCycle)

//VALUES

//(@MachineID, @Name, @CategoryID, @TypeID, @InstallationDate, @Status, @LifeCycle)";

//                SqlCommand insertCmd = new SqlCommand(insertMachineQuery, conn);

//                insertCmd.Parameters.AddWithValue("@MachineID", machine.MachineID);

//                insertCmd.Parameters.AddWithValue("@Name", machine.Name);

//                insertCmd.Parameters.AddWithValue("@CategoryID", machine.CategoryID);

//                insertCmd.Parameters.AddWithValue("@TypeID", machine.TypeID);

//                insertCmd.Parameters.AddWithValue("@InstallationDate", machine.InstallationDate);

//                insertCmd.Parameters.AddWithValue("@Status", machine.Status);

//                insertCmd.Parameters.AddWithValue("@LifeCycle", machine.LifeCycle); // ✅ Added

//                insertCmd.ExecuteNonQuery();

//                // Insert health metric

//                string getMaxIdQuery = "SELECT ISNULL(MAX(MetricID), 0) + 1 FROM HealthMetrics";

//                SqlCommand getIdCmd = new SqlCommand(getMaxIdQuery, conn);

//                int newMetricId = (int)getIdCmd.ExecuteScalar();

//                string insertMetricQuery = @"INSERT INTO HealthMetrics

//(MetricID, MachineID, CheckDate, Temperature, EnergyConsumption, HealthStatus)

//VALUES

//(@MetricID, @MachineID, @CheckDate, @Temperature, @EnergyConsumption, @HealthStatus)";

//                SqlCommand metricCmd = new SqlCommand(insertMetricQuery, conn);

//                metricCmd.Parameters.AddWithValue("@MetricID", newMetricId);

//                metricCmd.Parameters.AddWithValue("@MachineID", machine.MachineID);

//                metricCmd.Parameters.AddWithValue("@CheckDate", DateTime.Now);

//                metricCmd.Parameters.AddWithValue("@Temperature", machine.Temperature ?? 0);

//                metricCmd.Parameters.AddWithValue("@EnergyConsumption", machine.EnergyConsumption);

//                metricCmd.Parameters.AddWithValue("@HealthStatus", machine.HealthStatus);

//                metricCmd.ExecuteNonQuery();

//                return true;

//            }

//        }




public void Update(int id, Machine machine)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Update machine details
                string updateMachineQuery = @"UPDATE Machines SET 
                                      Name = @Name, 
                                      CategoryID = @CategoryID, 
                                      TypeID = @TypeID, 
                                      InstallationDate = @InstallationDate, 
                                      Status = @Status 
                                      WHERE MachineID = @MachineID";
                SqlCommand machineCmd = new SqlCommand(updateMachineQuery, conn);
                machineCmd.Parameters.AddWithValue("@MachineID", id);
                machineCmd.Parameters.AddWithValue("@Name", machine.Name);
                machineCmd.Parameters.AddWithValue("@CategoryID", machine.CategoryID);
                machineCmd.Parameters.AddWithValue("@TypeID", machine.TypeID);
                machineCmd.Parameters.AddWithValue("@InstallationDate", machine.InstallationDate);
                machineCmd.Parameters.AddWithValue("@Status", machine.Status);
                machineCmd.ExecuteNonQuery();

                // Update latest health metric for this machine
                string updateMetricQuery = @"UPDATE HealthMetrics SET
    Temperature = @Temperature, 
    EnergyConsumption = @EnergyConsumption, 
    CheckDate = @CheckDate, 
    HealthStatus = @HealthStatus
    WHERE MachineID = @MachineID AND MetricID = (
        SELECT TOP 1 MetricID 
        FROM HealthMetrics 
        WHERE MachineID = @MachineID 
        ORDER BY CheckDate DESC
    )";

                SqlCommand metricCmd = new SqlCommand(updateMetricQuery, conn);
                metricCmd.Parameters.AddWithValue("@MachineID", id);
                metricCmd.Parameters.AddWithValue("@Temperature", machine.Temperature ?? (object)DBNull.Value);
                metricCmd.Parameters.AddWithValue("@EnergyConsumption", machine.EnergyConsumption ?? (object)DBNull.Value);
                metricCmd.Parameters.AddWithValue("@CheckDate", DateTime.Now);
                metricCmd.Parameters.AddWithValue("@HealthStatus", machine.HealthStatus ?? (object)DBNull.Value);

                metricCmd.ExecuteNonQuery();
            }

            }


        //public void Delete(int id)
        //{
        //    using (SqlConnection conn = new SqlConnection(_connectionString))
        //    {

            //        string query = "DELETE FROM Machines WHERE MachineID = @MachineID";
            //        SqlCommand cmd = new SqlCommand(query, conn);
            //        cmd.Parameters.AddWithValue("@MachineID", id);
            //        conn.Open();
            //        cmd.ExecuteNonQuery();
            //    }
            //}

            //public void Delete(int id)
            //{
            //    using (SqlConnection conn = new SqlConnection(_connectionString))
            //    {
            //        conn.Open();

            //        // Check if the machine exists
            //        string checkQuery = "SELECT COUNT(*) FROM Machines WHERE MachineID = @MachineID";
            //        SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
            //        checkCmd.Parameters.AddWithValue("@MachineID", id);

            //        int count = (int)checkCmd.ExecuteScalar();

            //        if (count == 0)
            //        {
            //            // Machine does not exist
            //            throw new Exception("Machine not found. Cannot delete.");
            //        }

            //        // Proceed with deletion
            //        string deleteQuery = "DELETE FROM Machines WHERE MachineID = @MachineID";
            //        SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn);
            //        deleteCmd.Parameters.AddWithValue("@MachineID", id);
            //        deleteCmd.ExecuteNonQuery();
            //    }
            //}
        public bool Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Check if the machine exists
                string checkQuery = "SELECT COUNT(*) FROM Machines WHERE MachineID = @MachineID";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@MachineID", id);
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count == 0)
                    {
                        // Machine does not exist
                        return false;
                    }
                }

                // Proceed with deletion
                string deleteQuery = @"
            DELETE FROM TechnicianMachineAssignments WHERE MachineId = @MachineID;
            DELETE FROM MaintenanceLogs WHERE MachineID = @MachineID;
            DELETE FROM HealthMetrics WHERE MachineID = @MachineID;
            DELETE FROM Machines WHERE MachineID = @MachineID;";

                using (SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn))
                {
                    deleteCmd.Parameters.AddWithValue("@MachineID", id);
                    deleteCmd.ExecuteNonQuery();
                }

                return true;
            }
        }


    }
}
