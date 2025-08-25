using PMSCH.Server.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;

namespace PMSCH.Server.Repositories
{
    public class TokenRepository
    {
        private readonly string _connectionString;

        public TokenRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void SaveToken(Token token)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            var command = new SqlCommand("INSERT INTO Tokens (Token, UserId, Expiry) VALUES (@Token, @UserId, @Expiry)", connection);
            command.Parameters.AddWithValue("@Token", token.TokenValue);
            command.Parameters.AddWithValue("@UserId", token.UserId);
            command.Parameters.AddWithValue("@Expiry", token.Expiry);

            command.ExecuteNonQuery();
        }

        public bool IsValid(string tokenValue)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            var command = new SqlCommand("SELECT Expiry FROM Tokens WHERE Token = @Token", connection);
            command.Parameters.AddWithValue("@Token", tokenValue);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                var expiry = (DateTime)reader["Expiry"];
                return expiry > DateTime.UtcNow;
            }

            return false;
        }

        public void DeleteToken(string tokenValue)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            var command = new SqlCommand("DELETE FROM Tokens WHERE Token = @Token", connection);
            command.Parameters.AddWithValue("@Token", tokenValue);

            command.ExecuteNonQuery();
        }

        public void CleanupExpiredTokens()
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            var command = new SqlCommand("DELETE FROM Tokens WHERE Expiry < GETUTCDATE()", connection);
            command.ExecuteNonQuery();
        }
    }
}
