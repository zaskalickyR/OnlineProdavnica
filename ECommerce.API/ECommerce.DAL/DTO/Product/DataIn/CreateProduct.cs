using Microsoft.AspNetCore.Http;

namespace ECommerce.DAL.DTO.Product.DataIn
{
    public class CreateProduct
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; } = 0;
        public string? Description { get; set; }
        public int? CategoryId { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public IFormFile? Images { get; set; }
    }
}
