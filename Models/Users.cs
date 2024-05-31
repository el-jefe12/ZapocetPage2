using System.ComponentModel.DataAnnotations;

namespace TexasGuyContractIdentity.Models
{

    public class Users
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        
        public string Surname { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string UserAccess { get; set; }

    }
}
