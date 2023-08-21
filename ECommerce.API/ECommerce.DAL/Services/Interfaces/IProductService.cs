using ECommerce.DAL.DTO;
using ECommerce.DAL.DTO.Product.DataIn;
using ECommerce.DAL.DTO.Product.DataOut;
using ECommerce.DAL.DTO.User.DataIn;
using ECommerce.DAL.Models;

namespace ECommerce.DAL.Services.Interfaces
{
    public interface IProductService
    {
        Task<ResponsePackage<string>> Save(CreateProduct dataIn, int? userId);
        ResponsePackage<PaginationDataOut<ProductDataOut>> GetAll(PaginationDataIn dataIn, int? userId, string role);
        Task<ResponsePackage<string>> Delete(int userId);
        Task<ResponsePackage<ProductDataOut>> GetById(int productId);
        Task<ResponsePackage<List<ProductDataOut>>> GetProductByIds(List<int> ids);

    }
}
