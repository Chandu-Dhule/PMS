
using System.ComponentModel.DataAnnotations;

namespace PMSCH.Server.Models
{
    public class Login
    {

       

        public int     Id { get; set; }
        public string User { get; set; }
        public string Pass { get; set; } // Hashed password
        public string Role { get; set; }

        public int CategoryId { get; set; }


        }

    }
