using System.ComponentModel.DataAnnotations;

namespace TexasGuyContractIdentity.Models.API
{
    public class ApiToken
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? ExpiresAt { get; set; }
    }
}
