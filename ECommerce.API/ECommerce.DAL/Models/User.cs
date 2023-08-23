using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.DAL.Models
{
    public class User : Entity
    {
        [Required]
        public string? Address { get; set; }
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [Required]
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? UserName { get; set; }
        public string? Image { get; set; }
        public DateTime? BirthDate { get; set; }
        public Role Role { get; set; }
        public UserStatus Status { get; set; }
        public string? ActivateKey { get; set; }

        public async Task<string> SaveImage(IFormFile image)
        {
            if (image != null && image.Length > 0)
            {
                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                string imagePath = Path.Combine("C:\\Users\\Lenovo\\source\\repos\\OnlineProdavnica\\ECommerce.API\\Users\\ECommerce.Users\\images", uniqueFileName);

                using (var fileStream = new FileStream(imagePath, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }

                return "https://localhost:7219/images/" + uniqueFileName;
            }

            return null;
        }
    }
    public enum Role
    {
        Customer = 1,
        Saler = 2,
        Admin = 3,
    }

    public enum UserStatus
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2
    }


}