using ECommerce.DAL.Data;
using ECommerce.DAL.Repositories;

namespace ECommerce.DAL.UOWs
{
    public class UnitOfWorkUser : IUnitOfWorkUser
    {
        private UserDbContext _userDb;
        private IUserRepository UserRepository { get; set; }

        public UnitOfWorkUser(UserDbContext userDb)
        {
            _userDb = userDb;
        }

        public async Task<int> Save()
        {
            return await _userDb.SaveChangesAsync();
        }

        public IUserRepository GetUserRepository()
        {
            return UserRepository ?? (UserRepository = new UserRepository(_userDb));
        }
    }
}
