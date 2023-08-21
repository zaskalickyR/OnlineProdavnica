using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ECommerce.DAL.Models;
using ECommerce.DAL.DTO;

namespace ECommerce.DAL.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        User GetUserByEmailAndPassword(string email, string password);
        User GetUserByEmail(string email);
        User GetUserByUserName(string username);
        User GetUserById(int userId);
        List<User> GetUserByIds(List<int> ids);
        ResponsePackage<List<User>> GetAllUsersWithPagination(PaginationDataIn dataIn);
    }
}
