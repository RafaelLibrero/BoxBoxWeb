using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoxBoxModels
{
    [Table("Users")]

    public class User
    {
        [Key]
        [Column("UserID")]
        public int UserId { get; set; }
        [Required]
        [Column("UserName")]
        public string UserName { get; set; }
        [EmailAddress]
        [Column("Email")]
        public string? Email { get; set; }
        [Column("Password")]
        public byte[] Password { get; set; }
        [Column("RegistrationDate")]
        public DateTime RegistrationDate { get; set; }
        [Column("LastAccess")]
        public DateTime LastAccess { get; set; }
        [Column("RolID")]
        public int RolId { get; set; }
        [Column("ProfilePicture")]
        public string ProfilePicture { get; set; }
        [Column("TotalPosts")]
        public int TotalPosts { get; set; }
        [Column("TeamID")]
        public int? TeamId { get; set; }
        [Column("DriverID")]
        public int? DriverId { get; set; }
        [Column("Salt")]
        public string Salt { get; set; }
        [Column("Biography")]
        public string? Biography { get; set; }

    }
}
