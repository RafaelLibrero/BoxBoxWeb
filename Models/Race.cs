using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoxBoxModels
{
    [Table("Races")]
    public class Race
    {
        [Key]
        [Column("RaceID")]
        public int RaceId { get; set; }
        [Column("RaceName")]
        public string RaceName { get; set; }
        [Column("Image")]
        public string? Image { get; set; }
        [Column("Location")]
        public string? Location { get; set; }
        [NotMapped]
        public DateTime? StartDate => EndDate?.AddDays(-2);
        [Column("EndDate")]
        public DateTime? EndDate { get; set; }
        [Column("WinnerDriverID")]
        public int? WinnerDriverId { get; set; }

    }
}
