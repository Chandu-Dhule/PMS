using Microsoft.Extensions.Configuration;
using PMSCH.Server.Models;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace PMSCH.Server.Repositories
{
    public class MachineCategoryRepository
    {
        private readonly string _connectionString;

        public MachineCategoryRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        // ✅ Get all categories
        public List<MachineCategory> GetAll()
        {
            var categories = new List<MachineCategory>();

            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                string query = "SELECT CategoryID, CategoryName FROM MachineCategories";
                MySqlCommand cmd = new MySqlCommand(query, conn);

                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    categories.Add(new MachineCategory
                    {
                        CategoryID = Convert.ToInt32(reader["CategoryID"]),
                        CategoryName = reader["CategoryName"].ToString() ?? string.Empty
                    });
                }

                reader.Close();
            }

            return categories;
        }

        // ✅ Add a new category
        public void Add(MachineCategory category)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                string query = "INSERT INTO MachineCategories (CategoryID,CategoryName) VALUES (@CategoryID,@CategoryName)";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CategoryId", category.CategoryID);
                cmd.Parameters.AddWithValue("@CategoryName", category.CategoryName);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // ✅ Get category by ID
        public MachineCategory? GetById(int categoryId)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                string query = "SELECT CategoryID, CategoryName FROM MachineCategories WHERE CategoryID = @CategoryID";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CategoryID", categoryId);

                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new MachineCategory
                    {
                        CategoryID = Convert.ToInt32(reader["CategoryID"]),
                        CategoryName = reader["CategoryName"].ToString() ?? string.Empty
                    };
                }

                reader.Close();
            }

            return null;
        }

        // ✅ Delete category by ID
        public void Delete(int categoryId)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                string query = "DELETE FROM MachineCategories WHERE CategoryID = @CategoryID";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CategoryID", categoryId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
