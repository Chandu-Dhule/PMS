using PMSCH.Server.Models;
using System;
using System.Collections.Generic;

namespace PMSCH.Server.Repositories
{
    public interface IUserRepository
    {
        // User management
        User GetUserByUsername(string username);
        void AddUser(User user);
        IEnumerable<User> GetAllUsers();

        // 🔐 Token-based authentication
        void SaveToken(int userId, string token, DateTime expiry);
        bool ValidateToken(string token);
        void DeleteToken(string token);
        User GetUserByToken(string token);

        // Optional: Cleanup expired tokens
        void CleanupExpiredTokens();
    }
}
