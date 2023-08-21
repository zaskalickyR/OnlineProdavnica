using ECommerce.DAL.Repositories;

namespace ECommerce.DAL.UOWs
{
    public interface IUnitOfWorkProduct
    {
        ICategoryRepository GetCategoryRepository();
        IProductRepository GetProductRepository();
        IOrderRepository GetOrderRepository();

        Task<int> Save();
    }
}
