using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DAL.DTO.Product.DataOut
{
    public class ProductDataOut
    {
        public string Name { get; set; }
        public int? Id { get; set; }
        public double? Price { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        public string CategoryName { get; set; }
        public int? CategoryId { get; set; }
        public string Images { get; set; }
        public string Saler { get; set; }
        public int? CustomerId { get; set; }
        public DateTime? LastUpdateTime { get; set; }

        public ProductDataOut()
        {
        }

        public ProductDataOut(ECommerce.DAL.Models.Product product)
        {
            Name = product?.Name;
            Id = product?.Id;
            Stock = product.Stock;
            Price = product?.Price;
            Description = product?.Description;
            CategoryId = product?.CategoryId;
            Images = product?.Images;
            CategoryName = product?.Category?.Name;
            CustomerId = product.CustomerId;
            LastUpdateTime = product.LastUpdateTime;
        }
    }
}
