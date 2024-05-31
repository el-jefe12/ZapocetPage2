using System.ComponentModel.DataAnnotations;

namespace TexasGuyContractIdentity.Models.API
{
    public class ApiToken
    {
        [Key]
        public int ID { get; set; }

        public string Token { get; set; } 
    }
}
