using System.ComponentModel.DataAnnotations;

namespace ECommerce.DAL.DTO.User.DataIn
{
    public class LoginDataIn
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public int? AppVersion { get; set; }
    }
}
