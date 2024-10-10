using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using User_API.Enum;

namespace User_API.Entities
{
    [Table("usertable")]
    public class User
    {
        [Key] // Specifies that this is the primary key
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)] // used to auto-increment Id.
        [Required]
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Country { get; set; }

        public required UserStatus Status { get; set; }
        public required string CreatedAt { get; set; }        
        public required string UserName { get; set; } 

    }
}
