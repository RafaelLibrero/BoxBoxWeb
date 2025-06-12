using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoxBoxModels
{
    [Table("Topics")]
    public class Topic
    {
        [Key]
        [Column("TopicID")]
        public int TopicId { get; set; }
        [Column("Title")]
        public string Title { get; set; }
        [Column("Description")]
        public string Description { get; set; }
    }
}
