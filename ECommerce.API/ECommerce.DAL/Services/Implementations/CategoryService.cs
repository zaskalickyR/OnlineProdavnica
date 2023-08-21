using AutoMapper;
using ECommerce.DAL.Services.Interfaces;
using ECommerce.DAL.UOWs;
using ECommerce.DAL.DTO;
using ECommerce.DAL.DTO.Product.DataIn;
using ECommerce.DAL.Models;
using ECommerce.DAL.DTO.Product.DataOut;
using ECommerce.DAL.DTO.Category.DataOut;

namespace ECommerce.DAL.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly IEmailService _emailService;
        private readonly IUnitOfWorkProduct _unitOfWork;
        private readonly IMapper _mapper;


        public CategoryService(IEmailService userService, IUnitOfWorkProduct unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _emailService = userService;
            _mapper = mapper;
        }

        public ResponsePackage<List<CategoryDataOut>> GetAllForOption()
        {
            var categories = _unitOfWork.GetCategoryRepository().GetAllForOptions();
            //var data = _mapper.Map<List<CategoryDataOut>>(categories);
            var data = categories.Select(x=> new CategoryDataOut()
            {
                Id = x.Id,
                Name = x.Name,
            }).ToList();
            return new ResponsePackage<List<CategoryDataOut>>()
            {
                Status = ResponseStatus.Ok,
                TransferObject = data
            };
        }

        //public async Task<ResponsePackage<string>> Save(CreateProduct dataIn)
        //{
        //    var productForDb = new Product()
        //    {
        //        Name = dataIn.Name,
        //        Price = dataIn.Price,
        //        Stock = dataIn.Stock,
        //        Description = dataIn.Description,
        //        Images = dataIn.Images,
        //        CategoryId = dataIn.CategoryId
        //    };

        //    if(dataIn.Id == null || dataIn.Id == 0) //create new
        //    {
        //        if (_unitOfWork.GetProductRepository().GetProductByName(dataIn.Name).TransferObject != null)
        //            return new ResponsePackage<string>(ResponseStatus.Error, "Product with this name already exists.");

        //        await _unitOfWork.GetProductRepository().AddAsync(productForDb);
        //        _unitOfWork.Save();
        //        return new ResponsePackage<string>(ResponseStatus.Ok, "Successfully added new product.");
        //    }
        //    else // edit exist
        //    {
        //        var dbUser = await _unitOfWork.GetProductRepository().GetByIdAsync(dataIn.Id.GetValueOrDefault());
        //        if(dbUser == null)
        //            return new ResponsePackage<string>(ResponseStatus.Error, "Product not fount in database.");

        //        dbUser.Name = productForDb.Name;
        //        dbUser.Price = productForDb.Price;
        //        dbUser.Stock = productForDb.Stock;
        //        dbUser.Description = productForDb.Description;
        //        dbUser.CategoryId = productForDb.CategoryId;
        //        dbUser.Images = productForDb.Images;

        //        _unitOfWork.Save();
        //        return new ResponsePackage<string>(ResponseStatus.Ok, "Successfully edited product.");
        //    }
        //}

        
    }
}
