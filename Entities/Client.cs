using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace User_API.Entities
{
    [Table("clientable")]
    public class Client
    {

        [Required]
        [Key]
        [Column("ClientId")]
        public  int Id { get; set; }

        [StringLength(15, MinimumLength = 6, ErrorMessage = "UserName must be between 6 and 20 characters.")]
        public required string UserName { get; set; }

        public required string Password { get; set; }


    }
}
