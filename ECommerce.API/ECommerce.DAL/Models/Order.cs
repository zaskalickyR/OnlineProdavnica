using ECommerce.DAL.DTO.Order.DataIn;

namespace ECommerce.DAL.Models
{
    public class Order : Entity
    {
        public int? CustomerId { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Comment { get; set; }
        public DateTime ShippingTime { get; set; }
        public double? Shipping { get; set; }
        public double? Total { get; set; }
        public DateTime OrderDate { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public OrderStatus Status { get; set; }

    }
    public class OrderItem : Entity
    {
        public int? OrderId { get; set; }
        public Order Order { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }

    }

    public enum OrderStatus
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2,
        Completed = 3
    }
}