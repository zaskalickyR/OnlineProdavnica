using AutoMapper;
using ECommerce.DAL.DTO;
using ECommerce.DAL.DTO.Order.DataIn;
using ECommerce.DAL.DTO.Order.DataOut;
using ECommerce.DAL.DTO.Product.DataOut;
using ECommerce.DAL.DTO.User.DataOut;
using ECommerce.DAL.Models;
using ECommerce.DAL.Services.Interfaces;
using ECommerce.DAL.UOWs;
using EllipticCurve;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Org.BouncyCastle.Math.EC.Rfc7748;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DAL.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IEmailService _emailService;
        private readonly IUnitOfWorkProduct _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpClientService _httpClientService;


        public OrderService(IEmailService userService, IUnitOfWorkProduct unitOfWork, IMapper mapper, IHttpClientService httpClientService)
        {
            _unitOfWork = unitOfWork;
            _emailService = userService;
            _mapper = mapper;
            _httpClientService = httpClientService;
        }

        public async Task<ResponsePackage<string>> CancelOrder(int id)
        {
            var order = await _unitOfWork.GetOrderRepository().GetByIdAsync(id, includes: x => x.OrderItems);
            TimeSpan ts = DateTime.Now - order.OrderDate;
            if (order.Status == OrderStatus.Pending && (DateTime.Now < order.ShippingTime) && ts.TotalHours > 1)
            {
                var products = _unitOfWork.GetProductRepository().GetProductByIds(order.OrderItems.Select(x=>x.ProductId).ToList());
                foreach(var p in products)
                {
                    var count = order.OrderItems.FirstOrDefault(x => x.ProductId == p.Id).Quantity;
                    _unitOfWork.GetProductRepository().IncreseStock(p.Id, -count);
                }
                order.Status = OrderStatus.Rejected;
                await _unitOfWork.Save();
                var responseJson = await _httpClientService.PostDataToApi("https://localhost:7219/api/User/getByIds", JsonConvert.SerializeObject(new List<int>() { order.CustomerId.Value}));
                var listCustomers = JsonConvert.DeserializeObject<List<UserDataOut>>(responseJson);
                await _emailService.SendEmail(listCustomers[0].Email, "Vas order je uspesno cancelovan.", "Canceled order on Boom Shop");
                return new ResponsePackage<string>()
                {
                    Status = ResponseStatus.Ok,
                    Message = "Order successfully rejected!"
                };
            }
            else if(ts.TotalHours <= 1)
            {
                return new ResponsePackage<string>()
                {
                    Status = ResponseStatus.Error,
                    Message = "Order successfully rejected!"
                };
            }
            else if(DateTime.Now > order.ShippingTime)
            {
                return new ResponsePackage<string>()
                {
                    Status = ResponseStatus.Error,
                    Message = "Order is allready shipped"
                };
            }
            else
            {
                return new ResponsePackage<string>()
                {
                    Status = ResponseStatus.Error,
                    Message = "Error ocurred"
                };
            }
        }

        public async Task<ResponsePackage<PaginationDataOut<OrderDataOut>>> GetAll(PaginationDataIn dataIn, string role, int? userId)
        {
            var orders = _unitOfWork.GetOrderRepository().GetAllProductsWithPaggination(dataIn, role, userId);

            var ordersCustomersIds = orders.TransferObject.Select(x => x.CustomerId.Value).ToList();


            ordersCustomersIds.AddRange(orders.TransferObject
                                        .Where(order => order.OrderItems != null)
                                        .SelectMany(order => order.OrderItems)
                                        .Select(orderItem => orderItem.Product.CustomerId.Value)
                                        .ToList());

            var responseJson = await _httpClientService.PostDataToApi("https://localhost:7219/api/User/getByIds", JsonConvert.SerializeObject(ordersCustomersIds));
            var listCustomers = JsonConvert.DeserializeObject<List<UserDataOut>>(responseJson);


            List<Order> tempOrders = new List<Order>();
            if (role == "Saler")
            {
                foreach (var order in orders.TransferObject)
                {
                    var oiOfSaler = order.OrderItems.Where(x => x.Product.CustomerId == userId).ToList();
                    order.OrderItems = oiOfSaler;
                    order.Total = order.OrderItems.Sum(x => x.Product.Price*x.Quantity);
                    order.Shipping = 250;
                }
            }
            var ordersDto = orders.TransferObject.Select(x =>
            new OrderDataOut()
            {
                Customer = listCustomers.FirstOrDefault(y=>y.Id==x.CustomerId)?.FirstName + " " + listCustomers.FirstOrDefault(y => y.Id == x.CustomerId)?.LastName,
                CustomerId = x.CustomerId,
                Name = x.Name,
                Address = x.Address,
                Id = x.Id,
                Phone = x.Phone,
                Comment = x.Comment,
                Total = x.Total.Value,
                Shipping = x.Shipping.Value,
                ShippingTime = x.ShippingTime,
                OrderDate = x.OrderDate,
                Status = x.Status.ToString(),
                OrderItems = x.OrderItems.Select(y => new OrderItemDataOut(y, listCustomers)).ToList()
            }).ToList();



            return new ResponsePackage<PaginationDataOut<OrderDataOut>>()
            {
                Status = ResponseStatus.Ok,
                TransferObject = new PaginationDataOut<OrderDataOut> { Data = ordersDto, Count = int.Parse(orders.Message) }
            };
        }

        public async Task<ResponsePackage<string>> Save([FromBody] OrderDataIn dataIn, int? userId)
        {

            var numberSalerInOrder = dataIn.CartItems.Select(x => x.CustomerId)
                                                    .Distinct()
                                                    .Count();
            var tempList = _unitOfWork.GetProductRepository().GetProductByIds(dataIn.CartItems.Select(x => x.Id.GetValueOrDefault()).ToList());
            string orderErrors = "Price product with names: ";
            foreach(var item in tempList)
            {

                if (item.Price != dataIn.CartItems.FirstOrDefault(x => x.Id == item.Id).Price)
                    orderErrors += item.Name + ", ";
            }
            if (orderErrors != "Price product with names: ")
            {
                orderErrors += "is updated in meantime.";
                return new ResponsePackage<string>
                {
                    Status = ResponseStatus.Error,
                    Message = orderErrors,
                    TransferObject = orderErrors
                };
            }

            //stock umanjivanje
            foreach(var item in tempList)
            {
                var tempItem = dataIn.CartItems.FirstOrDefault(x => x.Name == item.Name);
                if (item.Stock >= tempItem.Count)
                    _unitOfWork.GetProductRepository().IncreseStock(tempItem.Id.Value, tempItem.Count.Value);
                else
                    return new ResponsePackage<string>()
                    {
                        Status = ResponseStatus.Error,
                        Message = "Error with stock of product with name " + tempItem.Name
                    };
            }
            //await _unitOfWork.Save();

            Random random = new Random();
            int randomHours = random.Next(3, 25);
            var newOrder = new Order()
            {
                Name = dataIn.FirstName + " " + dataIn.LastName,
                Comment = dataIn.Comment,
                CustomerId = userId,
                Phone = dataIn.PhoneNumber,
                ShippingTime = DateTime.Now.AddHours(randomHours),
                Status = OrderStatus.Pending,
                Address = dataIn.Address,
                LastUpdateTime = DateTime.Now,
                OrderDate = DateTime.Now, 
                OrderItems = dataIn.CartItems.Select(x => new OrderItem() { ProductId = x.Id.Value, Quantity = x.Count.Value }).ToList(),
                Shipping = numberSalerInOrder * 250,
                Total = (numberSalerInOrder * 250) + dataIn.CartItems.Sum(orderItem => orderItem.Price.Value * orderItem.Count.Value)
            };
            var responseJson = await _httpClientService.PostDataToApi("https://localhost:7219/api/User/getByIds", JsonConvert.SerializeObject(new List<int>() {userId.Value}));
            var listCustomers = JsonConvert.DeserializeObject<List<UserDataOut>>(responseJson);
            await _emailService.SendEmail(listCustomers[0].Email, "Uspesno ste napravili porudzbinu, mozete je proveriti u orders na nasoj stranici.", "New Order on Boom Shop");
            await _unitOfWork.GetOrderRepository().AddAsync(newOrder);
            await _unitOfWork.Save();
            return new ResponsePackage<string>(ResponseStatus.Ok, "Successfully ordered.");
        }
    }
}   