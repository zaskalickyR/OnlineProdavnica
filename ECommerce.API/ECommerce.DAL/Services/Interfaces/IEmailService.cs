using ECommerce.DAL.DTO;

namespace ECommerce.DAL.Services.Interfaces
{
    public interface IEmailService
    {
        Task<ResponsePackage<string>> SendEmail(string destination, string content, string title);
    }
}
