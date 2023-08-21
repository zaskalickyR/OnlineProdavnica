using Microsoft.EntityFrameworkCore;
using ECommerce.DAL.Models;
using ECommerce.DAL.Data;
using ECommerce.DAL.DTO;
using NLog.LayoutRenderers.Wrappers;

namespace ECommerce.DAL.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public ProductDbContext ProductDbContext
        {
            get { return _dbContext as ProductDbContext; }
        }

        public OrderRepository(ProductDbContext context) : base(context)
        {
        }

        public ResponsePackage<List<Order>> GetAllProductsWithPaggination(PaginationDataIn dataIn, string role, int? userId)
        {
            var q = _dbContext.Set<Order>().Include(x => x.OrderItems)
                                            .ThenInclude(x=>x.Product)
                                            .Where(x => x.IsDeleted == false);
            if (dataIn.SearchName != null && dataIn.SearchName != "")
                q = q.Where(x => x.Name.Contains(dataIn.SearchName) || x.Address.Contains(dataIn.SearchName) || x.Comment.Contains(dataIn.SearchName));
            var count = q.Count();

            Role tempRole;
            if (!Enum.TryParse<Role>(role, true, out tempRole))
                return new ResponsePackage<List<Order>>
                {
                    TransferObject = new List<Order>(),
                    Message = "0"
                };

            if (tempRole == Role.Customer)
                q = q.Where(x => x.CustomerId == userId);
            else if (tempRole == Role.Saler)
                q = q.Where(x => x.OrderItems.Any(y => y.Product.CustomerId == userId) || x.CustomerId == userId);

            if (dataIn.FilterByUserRole.Value)
            {
                var oneHourAgo = DateTime.Now.AddHours(-1);
                q = q.Where(x => x.Status == OrderStatus.Pending);
                q = q.Where(x => x.OrderDate >= oneHourAgo);
            }
            count = q.Count();
            return new ResponsePackage<List<Order>>
            {
                TransferObject = q.OrderByDescending(x => x.Id)
                    .Skip((dataIn.Page - 1) * dataIn.PageSize)
                    .Take(dataIn.PageSize).ToList(),
                Message = count.ToString()
            };
        }
    }
}
