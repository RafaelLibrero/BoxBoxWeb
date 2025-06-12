using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoxBoxModels
{
    [Table("V_Topics")]
    public class VTopic
    {
        [Key]
        [Column("TopicID")]
        public int TopicId { get; set; }
        [Column("Title")]
        public string Title { get; set; }
        [Column("Description")]
        public string Description { get; set; }
        [Column("Conversations")]
        public int Conversations { get; set; }
        [Column("Posts")]
        public int Posts { get; set; }
        [Column("LastMessage")]
        public int LastMessage { get; set; }
    }
}
