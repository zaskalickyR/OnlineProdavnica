using ECommerce.DAL.DTO;
using ECommerce.DAL.DTO.Category.DataOut;
using ECommerce.DAL.DTO.Product.DataIn;
using ECommerce.DAL.DTO.Product.DataOut;

namespace ECommerce.DAL.Services.Interfaces
{
    public interface ICategoryService
    {
        ResponsePackage<List<CategoryDataOut>> GetAllForOption();

    }
}
