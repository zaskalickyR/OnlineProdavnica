using ECommerce.DAL.Helpers;
using ECommerce.DAL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ECommerce.Product.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    [AllowAnonymous]
    public class BaseController : ControllerBase
    {
        public int? GetUserId()
        {
            var idClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "id")?.Value;
            return Int32.TryParse(idClaim, out int ret) ? ret : (int?)null;
        }
    }
}

