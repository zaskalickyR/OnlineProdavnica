using Microsoft.EntityFrameworkCore;
using ECommerce.DAL.Models;
using ECommerce.DAL.Data;
using ECommerce.DAL.DTO;

namespace ECommerce.DAL.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public ProductDbContext ProductDbContext
        {
            get { return _dbContext as ProductDbContext; }
        }

        public CategoryRepository(ProductDbContext context) : base(context)
        {
        }

        public List<Category> GetAllForOptions()
        {
            return _dbContext.Set<Category>().Where(x => x.IsDeleted == false).ToList();
        }
    }
}
