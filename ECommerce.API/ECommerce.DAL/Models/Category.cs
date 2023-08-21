using System.ComponentModel.DataAnnotations;

namespace ECommerce.DAL.Models
{
    public class Category : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }

    }
}