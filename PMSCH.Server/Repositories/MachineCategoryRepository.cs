using Microsoft.Extensions.Configuration;
using PMSCH.Server.Models;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

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

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT CategoryID, CategoryName FROM MachineCategories";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

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
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO MachineCategories (CategoryID,CategoryName) VALUES (@CategoryID,@CategoryName)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CategoryId", category.CategoryID);
                cmd.Parameters.AddWithValue("@CategoryName", category.CategoryName);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // ✅ Get category by ID
        public MachineCategory? GetById(int categoryId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT CategoryID, CategoryName FROM MachineCategories WHERE CategoryID = @CategoryID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CategoryID", categoryId);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

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
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM MachineCategories WHERE CategoryID = @CategoryID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CategoryID", categoryId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
