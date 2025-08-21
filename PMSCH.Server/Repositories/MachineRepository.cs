using Microsoft.Extensions.Configuration;
using PMSCH.Server.Models;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

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
                   hm.Temperature
            FROM Machines m
            LEFT JOIN HealthMetrics hm ON m.MachineID = hm.MachineID";

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
                        Temperature = reader["Temperature"] != DBNull.Value ? Convert.ToSingle(reader["Temperature"]) : (float?)null

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
                string query = @"
        SELECT m.MachineID, m.Name, m.CategoryID, m.TypeID, m.InstallationDate, m.Status,
               hm.Temperature
        FROM Machines m
        LEFT JOIN HealthMetrics hm ON m.MachineID = hm.MachineID
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
                        Temperature = reader["Temperature"] != DBNull.Value ? Convert.ToSingle(reader["Temperature"]) : (float?)null
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
               hm.Temperature
        FROM Machines m
        LEFT JOIN HealthMetrics hm ON m.MachineID = hm.MachineID
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
                string query = "SELECT * FROM Machines WHERE MachineID = @MachineID";
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
                        Status = reader["Status"].ToString()
                    };
                }

                reader.Close();
            }

            return null;
        }

        public void Add(Machine machine)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO Machines 
                                (MachineID, Name, CategoryID, TypeID, InstallationDate, Status) 
                                VALUES 
                                (@MachineID, @Name, @CategoryID, @TypeID, @InstallationDate, @Status)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MachineId", machine.MachineID);
                cmd.Parameters.AddWithValue("@Name", machine.Name);
                cmd.Parameters.AddWithValue("@CategoryID", machine.CategoryID);
                cmd.Parameters.AddWithValue("@TypeID", machine.TypeID);
                cmd.Parameters.AddWithValue("@InstallationDate", machine.InstallationDate);
                cmd.Parameters.AddWithValue("@Status", machine.Status);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(int id, Machine machine)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"UPDATE Machines SET 
                                Name = @Name, 
                                CategoryID = @CategoryID, 
                                TypeID = @TypeID, 
                                InstallationDate = @InstallationDate, 
                                Status = @Status 
                                WHERE MachineID = @MachineID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MachineID", id);
                cmd.Parameters.AddWithValue("@Name", machine.Name);
                cmd.Parameters.AddWithValue("@CategoryID", machine.CategoryID);
                cmd.Parameters.AddWithValue("@TypeID", machine.TypeID);
                cmd.Parameters.AddWithValue("@InstallationDate", machine.InstallationDate);
                cmd.Parameters.AddWithValue("@Status", machine.Status);
                conn.Open();
                cmd.ExecuteNonQuery();
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
        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
            DELETE FROM TechnicianMachineAssignments WHERE MachineId = @MachineID;
            DELETE FROM MaintenanceLogs WHERE MachineID = @MachineID;
            DELETE FROM HealthMetrics WHERE MachineID = @MachineID;
            DELETE FROM Machines WHERE MachineID = @MachineID;";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MachineID", id);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }


    }
}
