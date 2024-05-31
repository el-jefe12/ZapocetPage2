using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TexasGuyContractIdentity.Models
{
    public class History
    {
        [Key]
        public int ID { get; set; }

        public DateTime Timestamp { get; set; }

        public double Value { get; set; }

        public int StationID { get; set; }  // Foreign key

        [ForeignKey("StationID")]
        public virtual Stations Station { get; set; }  // Navigation property
    }
}
