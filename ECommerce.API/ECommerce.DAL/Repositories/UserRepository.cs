using Microsoft.EntityFrameworkCore;
using ECommerce.DAL.Models;
using ECommerce.DAL.Data;
using ECommerce.DAL.DTO;

namespace ECommerce.DAL.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserDbContext UserDbContext
        {
            get { return _dbContext as UserDbContext; }
        }

        public UserRepository(UserDbContext context) : base(context)
        {
        }

        public User GetUserByEmailAndPassword(string email, string password)
        {
             return _dbContext.Set<User>().FirstOrDefault(x => !x.IsDeleted && x.Email == email && x.Password == password);
        }

        public User GetUserByEmail(string email)
        {
            return _dbContext.Set<User>().FirstOrDefault(x => !x.IsDeleted && x.Email == email);
        }
        public User GetUserByUserName(string username)
        {
            return _dbContext.Set<User>().FirstOrDefault(x => !x.IsDeleted && x.UserName == username);
        }

        public User GetUserById(int userId)
        {
            return _dbContext.Set<User>().FirstOrDefault(x => !x.IsDeleted && x.Id == userId);
        }

        public ResponsePackage<List<User>> GetAllUsersWithPagination(PaginationDataIn dataIn)
        {
            var q = _dbContext.Set<User>().Where(x => x.IsDeleted == false);
            if (dataIn.SearchName != null && dataIn.SearchName != "")
                q = q.Where(x => x.FirstName.Contains(dataIn.SearchName) || x.LastName.Contains(dataIn.SearchName) || x.Email.Contains(dataIn.SearchName));

            if (dataIn.FilterByUserRole.GetValueOrDefault())
                q = q.Where(x => x.Role == Role.Saler);
            var count = q.Count();

            return new ResponsePackage<List<User>>
            {
                TransferObject = q.OrderByDescending(x => x.Id)
                    .Skip((dataIn.Page - 1) * dataIn.PageSize)
                    .Take(dataIn.PageSize).ToList(),
                Message = count.ToString()
            };
        }

        public List<User> GetUserByIds(List<int> ids)
        {
            return _dbContext.Set<User>().Where(x => ids.Contains(x.Id)).ToList();
        }
    }
}
