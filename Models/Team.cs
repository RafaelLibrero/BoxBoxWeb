using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoxBoxModels
{
    [Table("Teams")]
    public class Team
    {
        [Key]
        [Column("TeamID")]
        public int TeamId { get; set; }
        [Column("TeamName")]
        public string TeamName { get; set; }
        [Column("Logo")]
        public string Logo { get; set; }
        [Column("Points")]
        public int Points { get; set; }
    }
}
