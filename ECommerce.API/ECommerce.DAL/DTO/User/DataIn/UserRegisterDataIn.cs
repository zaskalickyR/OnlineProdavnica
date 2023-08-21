using Microsoft.AspNetCore.Http;

namespace ECommerce.DAL.DTO.User.DataIn
{
    public class RegisterUserDataIn
    {
        public int? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Username { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? RoleId { get; set; }
        public string? Password { get; set; }
        public DateTime? BirthDate { get; set; }
        public IFormFile? Image { get; set; }

    }
}
