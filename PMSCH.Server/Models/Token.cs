namespace PMSCH.Server.Models
{
    public class Token
    {
        public string TokenValue { get; set; }     // Primary key
        public int UserId { get; set; }            // Foreign key to Users
        public DateTime Expiry { get; set; }       // Expiration timestamp
    }
}
