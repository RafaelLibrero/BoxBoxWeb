using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoxBoxModels
{
    [Table("Drivers")]
    public class Driver
    {
        [Key]
        [Column("DriverID")]
        public int DriverID { get; set; }
        [Column("DriverName")]
        public string DriverName { get; set; }
        [Column("CarNumber")]
        public int CarNumber { get; set; }
        [Column("TeamID")]
        public int TeamID { get; set; }
        [Column("Flag")]
        public string Flag { get; set; }
        [Column("Imagen")]
        public string Imagen { get; set; }
        [Column("Points")]
        public int Points { get; set; }

    }
}
