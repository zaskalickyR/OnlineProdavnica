using ECommerce.DAL.Models;
using ECommerce.DAL.Data;
using ECommerce.DAL.DTO;
using ECommerce.DAL.DTO.Product.DataOut;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto;

namespace ECommerce.DAL.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductDbContext ProductDbContext
        {
            get { return _dbContext as ProductDbContext; }
        }

        public ProductRepository(ProductDbContext context) : base(context)
        {
        }

        public ResponsePackage<ProductDataOut> GetProductByName(string Name)
        {
            var tempUser = _dbContext.Set<Product>().FirstOrDefault(x => !x.IsDeleted && x.Name == Name);
            return new ResponsePackage<ProductDataOut>()
            {

                TransferObject = (tempUser != null ? new ProductDataOut(tempUser) : null)
            };
        }

        public ResponsePackage<List<Product>> GetAllProductsWithPaggination(PaginationDataIn dataIn, int? userId, string role)
        {
            var q = _dbContext.Set<Product>().Include(x=>x.Category).Where(x => x.IsDeleted==false);
            if (dataIn.SearchName != null && dataIn.SearchName != "")
                q = q.Where(x => x.Name.Contains(dataIn.SearchName));

            var count = q.Count();

            if(dataIn.FilterByUserRole != null && dataIn.FilterByUserRole != false) {
                Role tempRole;
                if (!Enum.TryParse<Role>(role, true, out tempRole))
                    return new ResponsePackage<List<Product>>
                    {
                        TransferObject = new List<Product>(),   //losa rola ovo je nemoguce ali je zastita
                        Message = "0"
                    };
                if (tempRole == Role.Saler)
                {
                    q = q.Where(x => x.CustomerId == userId); //vrati mu sve njegove proizvode
                    count = q.Count();
                }
            }

            return new ResponsePackage<List<Product>>
            {
                TransferObject = q.OrderByDescending(x => x.Id)
                    .Skip((dataIn.Page - 1) * dataIn.PageSize)
                    .Take(dataIn.PageSize).ToList(),
                Message = count.ToString()
            };
        }

        public List<Product> GetProductByIds(List<int> ids)
        { 
            var q = _dbContext.Set<Product>().Where(x => ids.Any(y => y == x.Id));
            return q.ToList();
        }

        public bool IncreseStock(int id, int count)
        {
            var q = _dbContext.Set<Product>().FirstOrDefault(x => x.IsDeleted == false && x.Id == id);
            if (q.Stock >= count && count>0)
            {
                q.Stock = q.Stock - count;
                _dbContext.SaveChanges();
                return true;
            }
            else if(count<=0)
            {
                q.Stock = q.Stock - count;
                _dbContext.SaveChanges();
                return true;
            }
            else
                return false;
        }
    }
}
