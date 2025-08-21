//using System.ComponentModel.DataAnnotations;

//namespace PMSCH.Server.Models
//{
//    public class Login
//    {
//    }
//}
using System.ComponentModel.DataAnnotations;

namespace PMSCH.Server.Models
{
    public class Login
    {

        //[Key]
        //public string User { get; set; }

        //[Required]
        //public string Pass { get; set; }

        //[Required]
        //public string Role { get; set; }

        public int     Id { get; set; }
        public string User { get; set; }
        public string Pass { get; set; } // Hashed password
        public string Role { get; set; }

        public int CategoryId { get; set; }


        }

    }
