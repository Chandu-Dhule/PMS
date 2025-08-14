using Microsoft.Extensions.Configuration;
using PMSCH.Server.Models;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace PMSCH.Server.Repositories
{
    public class MachineTypeRepository
    {
        private readonly string _connectionString;

        public MachineTypeRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        // ✅ Get all machine types
        public List<MachineType> GetAll()
        {
            var types = new List<MachineType>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT TypeID, TypeName, Description FROM MachineTypes";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    types.Add(new MachineType
                    {
                        TypeID = Convert.ToInt32(reader["TypeID"]),
                        TypeName = reader["TypeName"].ToString() ?? string.Empty,
                        Description = reader["Description"].ToString() ?? string.Empty
                    });
                }

                reader.Close();
            }

            return types;
        }

        // ✅ Add a new machine type
        public void Add(MachineType type)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO MachineTypes (TypeName, Description) VALUES (@TypeName, @Description)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TypeName", type.TypeName);
                cmd.Parameters.AddWithValue("@Description", type.Description);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // ✅ Get machine type by ID
        public MachineType? GetById(int typeId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT TypeID, TypeName, Description FROM MachineTypes WHERE TypeID = @TypeID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TypeID", typeId);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new MachineType
                    {
                        TypeID = Convert.ToInt32(reader["TypeID"]),
                        TypeName = reader["TypeName"].ToString() ?? string.Empty,
                        Description = reader["Description"].ToString() ?? string.Empty
                    };
                }

                reader.Close();
            }

            return null;
        }

        // ✅ Delete machine type by ID
        public void Delete(int typeId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM MachineTypes WHERE TypeID = @TypeID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TypeID", typeId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
