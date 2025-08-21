using PMSCH.Server.Models;
using PMSCH.Server.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

public class UserRepository : IUserRepository
{
    private readonly string _connectionString;
    private readonly TechnicianMachineAssignmentRepository _assignmentRepository;

    public UserRepository(IConfiguration configuration, TechnicianMachineAssignmentRepository assignmentRepository)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
        _assignmentRepository = assignmentRepository;
    }

    public User GetUserByUsername(string username)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var command = new SqlCommand("SELECT * FROM Logins WHERE [User] = @Username", connection);
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

        var command = new SqlCommand(@"
            INSERT INTO Logins (Username, PasswordHash, Role, CategoryID)
            VALUES (@Username, @PasswordHash, @Role, @CategoryID)", connection);

        command.Parameters.AddWithValue("@Username", user.Username);
        command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
        command.Parameters.AddWithValue("@Role", user.Role);
        command.Parameters.AddWithValue("@CategoryID", user.CategoryID ?? (object)DBNull.Value);

        command.ExecuteNonQuery();

        // Assign machines if Technician
        if (user.Role == "Technician" && user.AssignedMachineIds != null && user.AssignedMachineIds.Any())
        {
            var createdUser = GetUserByUsername(user.Username);
            if (createdUser != null)
            {
                foreach (var machineId in user.AssignedMachineIds)
                {
                    _assignmentRepository.AssignMachine(createdUser.Id, machineId);
                }
            }
        }
    }

    public IEnumerable<User> GetAllUsers()
    {
        var users = new List<User>();

        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var command = new SqlCommand("SELECT * FROM Logins", connection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            users.Add(MapUser(reader));
        }

        return users;
    }

    //public void SaveToken(int userId, string token, DateTime expiry)
    //{
    //    using var connection = new SqlConnection(_connectionString);
    //    connection.Open();

    //    var command = new SqlCommand(@"
    //        INSERT INTO Tokens (UserId, Token, Expiry)
    //        VALUES (@UserId, @Token, @Expiry)", connection);

    //    command.Parameters.AddWithValue("@UserId", userId);
    //    command.Parameters.AddWithValue("@Token", token);
    //    command.Parameters.AddWithValue("@Expiry", expiry);

    //    command.ExecuteNonQuery();
   // }

    public bool ValidateToken(string token)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var command = new SqlCommand("SELECT Expiry FROM Tokens WHERE Token = @Token", connection);
        command.Parameters.AddWithValue("@Token", token);

        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            var expiry = (DateTime)reader["Expiry"];
            return expiry > DateTime.UtcNow;
        }

        return false;
    }

    public User GetUserByToken(string token)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var command = new SqlCommand(@"
            SELECT u.Id, u.Username, u.PasswordHash, u.Role, u.CategoryID
            FROM Logins u
            INNER JOIN Tokens t ON u.Id = t.UserId
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

        var command = new SqlCommand("DELETE FROM Tokens WHERE Token = @Token", connection);
        command.Parameters.AddWithValue("@Token", token);

        command.ExecuteNonQuery();
    }

    public void CleanupExpiredTokens()
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var command = new SqlCommand("DELETE FROM Tokens WHERE Expiry < GETUTCDATE()", connection);
        command.ExecuteNonQuery();
    }

    private User MapUser(SqlDataReader reader)
    {
        var user = new User
        {
            Id = (int)reader["Id"],
            Username = reader["User"].ToString(),
            PasswordHash = reader["Pass"].ToString(),
            Role = reader["Role"].ToString(),
            CategoryID = reader["CategoryID"] == DBNull.Value ? null : (int?)reader["CategoryID"]
        };

        if (user.Role == "Technician")
        {
            user.AssignedMachineIds = _assignmentRepository.GetAssignedMachines(user.Id);
        }

        return user;
    }

    //-------------------------------------
    public Login GetLogin(string user)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var command = new SqlCommand("SELECT * FROM Logins WHERE [User] = @User", connection);
        command.Parameters.AddWithValue("@User", user);

        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return new Login
            {
                Id = (int)reader["Id"],
                User = reader["User"].ToString(),
                Pass = reader["Pass"].ToString(),
                Role = reader["Role"].ToString(),
                CategoryId = reader["CategoryID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["CategoryID"])
            };
        }

        return null;
    }


    public void CreateLogin(Login login)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var command = new SqlCommand("INSERT INTO Logins ([User], [Pass], [Role]) VALUES (@User, @Pass, @Role)", connection);
        command.Parameters.AddWithValue("@User", login.User);
        command.Parameters.AddWithValue("@Pass", login.Pass);
        command.Parameters.AddWithValue("@Role", login.Role);


        command.ExecuteNonQuery();
    }

    public List<Login> GetAll()
    {
        var login = new List<Login>();
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            string query = "SELECT * FROM Logins";
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                login.Add(new Login
                {
                    Id = (int)reader["Id"],
                    User = reader["User"].ToString(),
                    Pass = reader["Pass"].ToString(),
                    Role = reader["Role"].ToString(),
                    CategoryId = reader["CategoryID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["CategoryID"])
                });
            }
        }
        return login;
    }


}
