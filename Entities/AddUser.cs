using System.ComponentModel.DataAnnotations;
using User_API.Enum;


namespace User_API.Entities
{
    public class AddUser
    {

        [StringLength(20, MinimumLength = 2, ErrorMessage = "Max length =20 and minimum length=3")]
        public required string Name { get; set; }

        [StringLength(20, MinimumLength = 4, ErrorMessage = "Max length =50 and minimum length=4")]
        public required string Country { get; set; }

        
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Max length =20 and minimum length=6")]
        public required string UserName { get; set; }

        public required string Password { get; set; }
    }
}
