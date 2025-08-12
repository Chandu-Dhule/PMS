using PMSCH.Server.Repositories;

namespace PMSCH.Server.Healper
{
    public static class TokenValidator
    {
        public static bool IsTokenValid(HttpRequest request, IUserRepository userRepo)
        {
            var token = request.Headers["X-Custom-Token"].FirstOrDefault();
            if (string.IsNullOrEmpty(token)) return false;

            return userRepo.ValidateToken(token);
        }
    }

}
