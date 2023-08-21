using ECommerce.DAL.Data;
using ECommerce.DAL.Repositories;

namespace ECommerce.DAL.UOWs
{
    public class UnitOfWorkProduct : IUnitOfWorkProduct
    {
        private ProductDbContext _productDb;
        private ICategoryRepository CategoryRepository { get; set; }
        private IProductRepository ProductRepository { get; set; }
        private IOrderRepository OrderRepository { get; set; }

        public UnitOfWorkProduct(ProductDbContext productDb)
        {
            _productDb = productDb;
        }

        public async Task<int> Save()
        {
            return await _productDb.SaveChangesAsync();
        }
        public ICategoryRepository GetCategoryRepository()
        {
            return CategoryRepository ?? (CategoryRepository = new CategoryRepository(_productDb));
        }
        public IProductRepository GetProductRepository()
        {
            return ProductRepository ?? (ProductRepository = new ProductRepository(_productDb));
        }
        public IOrderRepository GetOrderRepository()
        {
            return OrderRepository ?? (OrderRepository = new OrderRepository(_productDb));
        }
    }
}
