using ECommerce.DAL.DTO.Product.DataIn;
using ECommerce.DAL.DTO;
using ECommerce.DAL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ECommerce.DAL.Services.Implementations;

namespace ECommerce.Product.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public int? GetUserId()
        {
            var idClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "id")?.Value;
            return Int32.TryParse(idClaim, out int ret) ? ret : (int?)null;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public string GetUserRole()
        {
            return HttpContext.User.Claims.FirstOrDefault(x => x.Type.Contains("role"))?.Value;
            //return Int32.TryParse(idClaim, out int ret) ? ret : (int?)null;
        }
        [HttpPost("save")]
        [DisableRequestSizeLimit]
        public ActionResult Save([FromForm] CreateProduct dataIn)
        {
            return Ok(_productService.Save(dataIn, GetUserId()));
        }
        
        [HttpPost("getAll")]
        public ActionResult GetAll([FromBody] PaginationDataIn dataIn)
        {
            return Ok(_productService.GetAll(dataIn, GetUserId(), GetUserRole()));
        }
        [HttpGet("delete/{productId}")]
        public ActionResult Delete(int productId)
        {
            return Ok(_productService.Delete(productId));
        }
        [HttpGet("get/{productId}")]
        public async Task<ActionResult> Get(int productId)
        {
            return Ok(await _productService.GetById(productId));
        }
        [HttpPost("updateCart")]
        public async Task<ActionResult> Get([FromBody] List<int> productIds)
        {
            return Ok(await _productService.GetProductByIds(productIds));
        }
    }
}
