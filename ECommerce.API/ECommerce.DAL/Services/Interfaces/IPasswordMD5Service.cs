using ECommerce.DAL.DTO;
using ECommerce.DAL.DTO.Category.DataOut;
using ECommerce.DAL.DTO.Product.DataIn;
using ECommerce.DAL.DTO.Product.DataOut;

namespace ECommerce.DAL.Services.Interfaces
{
    public interface IPasswordMD5Service
    {
        string GetMd5Hash(string input);
        bool VerifyMd5Hash(string input, string hash);
    }
}
