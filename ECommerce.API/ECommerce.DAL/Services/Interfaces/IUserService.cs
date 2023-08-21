using ECommerce.DAL.DTO;
using ECommerce.DAL.DTO.User.DataIn;
using ECommerce.DAL.DTO.User.DataOut;
using ECommerce.DAL.Models;

namespace ECommerce.DAL.Services.Interfaces
{
    public interface IUserService
    {
        Task<ResponsePackage<string>> Save(RegisterUserDataIn dataIn);
        ResponsePackage<User> GetUserByEmailAndPass(string email, string pass);
        ResponsePackage<string> ActivateUser(string email, string key);
        ResponsePackage<string> ApproveOrRejectUser(int userId, bool rejectOrApprove);
        ResponsePackage<string> Delete(int userId);
        List<UserDataOut> GetByIds(List<int> ids);
        ResponsePackage<UserDataOut> Get(int userId);
        ResponsePackage<PaginationDataOut<UserDataOut>> GetAll(PaginationDataIn dataIn);
        Task<User> RegisterOrLoginFacebookUser(FacebookUserData dataIn);

    }
}
