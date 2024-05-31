using System.ComponentModel.DataAnnotations;

namespace TexasGuyContractIdentity.Models
{
    public class Stations
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string ?StationName { get; set; }

        public int Minutes { get; set; } // Non-nullable

        public double LowestLevel { get; set; } // Non-nullable

        public double HighestLevel { get; set; } // Non-nullable

        public bool isEnabled { get; set; }

        [EmailAddress]
        public string ?Email { get; set; } // Nullable
    }
}
