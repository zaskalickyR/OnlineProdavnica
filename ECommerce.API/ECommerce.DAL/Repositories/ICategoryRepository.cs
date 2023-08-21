using ECommerce.DAL.Data;
using ECommerce.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.DAL.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        public List<Category> GetAllForOptions();
    }
}
