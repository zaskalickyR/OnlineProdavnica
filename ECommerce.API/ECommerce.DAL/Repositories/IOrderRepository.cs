using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ECommerce.DAL.Models;
using ECommerce.DAL.DTO;

namespace ECommerce.DAL.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        ResponsePackage<List<Order>> GetAllProductsWithPaggination(PaginationDataIn dataIn, string role, int? userId);

        
    }
}
