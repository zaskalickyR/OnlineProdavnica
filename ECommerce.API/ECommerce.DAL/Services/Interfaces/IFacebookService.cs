using ECommerce.DAL.DTO.User.DataIn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DAL.Services.Interfaces
{
    public interface IFacebookService
    {
        Task<FacebookUserData> GetUserData(string accessToken);
    }
}
