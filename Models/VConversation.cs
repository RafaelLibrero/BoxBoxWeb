using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoxBoxModels
{
    [Table("V_Conversations")]
    public class VConversation
    {
        [Key]
        [Column("ConversationID")]
        public int ConversationId { get; set; }
        [Column("TopicID")]
        public int TopicId { get; set; }
        [Column("UserID")]
        public int UserId { get; set; }
        [Column("Title")]
        public string Title { get; set; }
        [Column("EntryCount")]
        public int EntryCount { get; set; }
        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; }
        [Column("PostCount")]
        public int PostCount { get; set; }
        [Column("LastMessage")]
        public int LastMessage { get; set; }
    }
}
