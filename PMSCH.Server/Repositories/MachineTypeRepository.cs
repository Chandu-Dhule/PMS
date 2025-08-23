using Microsoft.Extensions.Configuration;
using PMSCH.Server.Models;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;

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

            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                string query = "SELECT TypeID, TypeName, Description FROM MachineTypes";
                MySqlCommand cmd = new MySqlCommand(query, conn);

                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

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
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                string query = "INSERT INTO MachineTypes (TypeName, Description) VALUES (@TypeName, @Description)";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TypeName", type.TypeName);
                cmd.Parameters.AddWithValue("@Description", type.Description);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // ✅ Get machine type by ID
        public MachineType? GetById(int typeId)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                string query = "SELECT TypeID, TypeName, Description FROM MachineTypes WHERE TypeID = @TypeID";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TypeID", typeId);

                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

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
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                string query = "DELETE FROM MachineTypes WHERE TypeID = @TypeID";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TypeID", typeId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
