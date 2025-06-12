using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoxBoxModels
{
    [Table("Posts")]
    public class Post
    {
        [Key]
        [Column("PostID")]
        public int PostId { get; set; }
        [Column("ConversationID")]
        public int ConversationId { get; set; }
        [Column("UserID")]
        public int UserId { get; set; }
        [Column("Text")]
        public string Text { get; set; }
        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; }
        [Column("Estado")]
        public int Estado { get; set; }

    }
}
