
using PMSCH.Server.Models;
using PMSCH.Server.Repositories;

using Microsoft.Data.SqlClient;


public class UserRepository : IUserRepository
{
    private readonly string _connectionString;

    public UserRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public User GetUserByUsername(string username)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var command = new SqlCommand("SELECT * FROM Users WHERE Username = @Username", connection);
        command.Parameters.AddWithValue("@Username", username);

        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return MapUser(reader);
        }

        return null;
    }

    public void AddUser(User user)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var command = new SqlCommand("INSERT INTO Users (Username, PasswordHash, Role, Category, AssignedMachineIds) VALUES (@Username, @PasswordHash, @Role, @Category, @AssignedMachineIds)", connection);
        command.Parameters.AddWithValue("@Username", user.Username);
        command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
        command.Parameters.AddWithValue("@Role", user.Role);
        command.Parameters.AddWithValue("@Category", user.Category ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@AssignedMachineIds", string.Join(",", user.AssignedMachineIds ?? new List<int>()));

        command.ExecuteNonQuery();
    }

    public IEnumerable<User> GetAllUsers()
    {
        var users = new List<User>();

        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var command = new SqlCommand("SELECT * FROM Users", connection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            users.Add(MapUser(reader));
        }

        return users;
    }

    public void SaveToken(int userId, string token, DateTime expiry)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var command = new SqlCommand("INSERT INTO UserTokens (UserId, Token, Expiry) VALUES (@UserId, @Token, @Expiry)", connection);
        command.Parameters.AddWithValue("@UserId", userId);
        command.Parameters.AddWithValue("@Token", token);
        command.Parameters.AddWithValue("@Expiry", expiry);

        command.ExecuteNonQuery();
    }

    public bool ValidateToken(string token)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var command = new SqlCommand("SELECT Expiry FROM UserTokens WHERE Token = @Token", connection);
        command.Parameters.AddWithValue("@Token", token);

        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            var expiry = (DateTime)reader["Expiry"];
            return expiry > DateTime.Now;
        }

        return false;
    }

    public User GetUserByToken(string token)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var command = new SqlCommand(@"
            SELECT u.Id, u.Username, u.PasswordHash, u.Role, u.Category, u.AssignedMachineIds
            FROM Users u
            INNER JOIN UserTokens t ON u.Id = t.UserId
            WHERE t.Token = @Token", connection);

        command.Parameters.AddWithValue("@Token", token);

        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return MapUser(reader);
        }

        return null;
    }
    public void DeleteToken(string token)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var command = new SqlCommand("DELETE FROM UserTokens WHERE Token = @Token", connection);
        command.Parameters.AddWithValue("@Token", token);

        command.ExecuteNonQuery();
    }


    private User MapUser(SqlDataReader reader)
    {
        return new User
        {
            Id = (int)reader["Id"],
            Username = reader["Username"].ToString(),
            PasswordHash = reader["PasswordHash"].ToString(),
            Role = reader["Role"].ToString(),
            Category = reader["Category"]?.ToString(),
            AssignedMachineIds = reader["AssignedMachineIds"]?.ToString()?.Split(',')?.Select(int.Parse)?.ToList() ?? new List<int>()
        };
    }
}
