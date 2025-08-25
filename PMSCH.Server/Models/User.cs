namespace PMSCH.Server.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; } // Admin, Manager, Technician
        public int? CategoryID { get; set; } // Nullable for non-managers
        // For Managers

        // Not stored in Users table; populated from TechnicianMachineAssignments
        public List<int> AssignedMachineIds { get; set; } = new List<int>();
    }
}
