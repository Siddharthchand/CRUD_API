using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace User_API.Entities
{
    [Table("verifyclientable")]

    public class VerifyClient
    {
        [Key]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Max length =20 and minimum length=6")]
        public required string ClientId { get; set; }

        [StringLength(20, MinimumLength = 6, ErrorMessage = "Max length =20 and minimum length=6")]
        public required string ClientSecret { get; set; }

       
    }
}
