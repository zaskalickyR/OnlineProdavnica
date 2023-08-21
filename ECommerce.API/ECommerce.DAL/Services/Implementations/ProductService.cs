using ECommerce.DAL.Data;
using ECommerce.DAL.DTO;
using ECommerce.DAL.DTO.Product.DataIn;
using ECommerce.DAL.DTO.User.DataIn;
using ECommerce.DAL.Services.Interfaces;
using ECommerce.DAL.UOWs;
using ECommerce.DAL.Models;
using Newtonsoft.Json;
using AutoMapper;
using ECommerce.DAL.DTO.Product.DataOut;

namespace ECommerce.DAL.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly ProductDbContext _dbContext;
        private readonly IEmailService _emailService;
        private readonly IUnitOfWorkProduct _unitOfWork;
        private readonly IMapper _mapper;


        public ProductService(ProductDbContext dbContext, IEmailService userService, IUnitOfWorkProduct unitOfWork, IMapper mapper)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
            _emailService = userService;
            _mapper = mapper;
        }

        public async Task<ResponsePackage<string>> Delete(int userId)
        {
            var tempProduct = await _unitOfWork.GetProductRepository().GetByIdAsync(userId);
            if (tempProduct == null)
                return new ResponsePackage<string>
                {
                    Message = "Product doesn't exist in database!",
                    Status = 200
                };
            else
            {
                tempProduct.IsDeleted = true;
                _unitOfWork.Save();
                return new ResponsePackage<string>
                {
                    Message = "Product has been successfully deleted!",
                    Status = 200
                };
            }
        }

        public ResponsePackage<PaginationDataOut<ProductDataOut>> GetAll(PaginationDataIn dataIn, int? userId, string role)
        {
            var products = _unitOfWork.GetProductRepository().GetAllProductsWithPaggination(dataIn, userId, role);
            var data = products.TransferObject.Select(x => new ProductDataOut(x)).ToList();

            return new ResponsePackage<PaginationDataOut<ProductDataOut>>()
            {
                Status = ResponseStatus.Ok,
                TransferObject = new PaginationDataOut<ProductDataOut> { Data = data, Count = int.Parse(products.Message) }
            };
        }

        public async Task<ResponsePackage<ProductDataOut>> GetById(int productId)
        {
            var tempProduct = await _unitOfWork.GetProductRepository().GetByIdAsync(productId);
            if (tempProduct == null)
                return new ResponsePackage<ProductDataOut>
                {
                    Status = ResponseStatus.Error,
                    Message = "Product don't exist in database."
                };
            else
                return new ResponsePackage<ProductDataOut>
                {
                    TransferObject = new ProductDataOut(tempProduct),
                    Status = ResponseStatus.Ok,
                };
        }

        public async Task<ResponsePackage<List<ProductDataOut>>> GetProductByIds(List<int> ids)
        {
            return new ResponsePackage<List<ProductDataOut>>
            {
                Status = ResponseStatus.Ok,
                TransferObject = _unitOfWork.GetProductRepository().GetProductByIds(ids).Select(x => new ProductDataOut(x)).ToList(),
            };
        }

        public async Task<ResponsePackage<string>> Save(CreateProduct dataIn, int? userId)
        {
            var productForDb = new Product()
            {
                Name = dataIn.Name,
                Price = dataIn.Price,
                Stock = dataIn.Stock,
                Description = dataIn.Description,
                CategoryId = dataIn.CategoryId,
                LastUpdateTime = DateTime.Now,
                CustomerId = userId
            };
            productForDb.Images = await productForDb.SaveImage(dataIn.Images);

            if(dataIn.Id == null || dataIn.Id == 0) //create new
            {
                if (_unitOfWork.GetProductRepository().GetProductByName(dataIn.Name).TransferObject != null)
                    return new ResponsePackage<string>(ResponseStatus.Error, "Product with this name already exists.");

                await _unitOfWork.GetProductRepository().AddAsync(productForDb);
                await _unitOfWork.Save();
                return new ResponsePackage<string>(ResponseStatus.Ok, "Successfully added new product.");
            }
            else // edit exist
            {
                var dbUser = await _unitOfWork.GetProductRepository().GetByIdAsync(dataIn.Id.GetValueOrDefault());
                if(dbUser == null)
                    return new ResponsePackage<string>(ResponseStatus.Error, "Product not fount in database.");

                if (dataIn.LastUpdateTime != dbUser.LastUpdateTime)
                    return new ResponsePackage<string>(ResponseStatus.Error, "The product has been changed in the meantime. Try again.");

                dbUser.Name = productForDb.Name;
                dbUser.Price = productForDb.Price;
                dbUser.Stock = productForDb.Stock;
                dbUser.Description = productForDb.Description;
                dbUser.CategoryId = productForDb.CategoryId;
                if(dataIn.Images !=null)
                    dbUser.Images = await dbUser.SaveImage(dataIn.Images);
                dbUser.LastUpdateTime = DateTime.Now;

                await _unitOfWork.Save();
                return new ResponsePackage<string>(ResponseStatus.Ok, "Successfully edited product.");
            }
        }

        
    }
}
