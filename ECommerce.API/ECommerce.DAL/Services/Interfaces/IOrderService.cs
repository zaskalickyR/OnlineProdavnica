using ECommerce.DAL.DTO;
using ECommerce.DAL.DTO.Category.DataOut;
using ECommerce.DAL.DTO.Order.DataIn;
using ECommerce.DAL.DTO.Order.DataOut;
using ECommerce.DAL.DTO.Product.DataIn;
using ECommerce.DAL.DTO.Product.DataOut;

namespace ECommerce.DAL.Services.Interfaces
{
    public interface IOrderService
    {
        Task<ResponsePackage<string>> Save(OrderDataIn dataIn, int? userId);
        Task<ResponsePackage<PaginationDataOut<OrderDataOut>>> GetAll(PaginationDataIn dataIn, string role, int? userId);
        Task<ResponsePackage<string>> CancelOrder(int id);
    }
}
