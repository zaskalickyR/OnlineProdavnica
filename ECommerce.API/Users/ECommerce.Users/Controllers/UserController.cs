using ECommerce.DAL.DTO;
using ECommerce.DAL.DTO.User.DataIn;
using ECommerce.DAL.Services.Implementations;
using ECommerce.DAL.Services.Interfaces;
using ECommerce.DAL.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using ECommerce.DAL.Models;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Facebook;
using System.Diagnostics.CodeAnalysis;

namespace ECommerce.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IFacebookService _facebookService;
        private readonly IUserService _userService;
        private readonly IPasswordMD5Service _passwordService;
        private readonly IEmailService _emailService;


        public UserController(IUserService userService, IEmailService emailService, IFacebookService facebookService)
        {
            this._userService = userService;
            this._emailService = emailService;
            this._facebookService = facebookService;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public int? GetUserId()
        {
            var idClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "id")?.Value;
            return Int32.TryParse(idClaim, out int ret) ? ret : (int?)null;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public ActionResult Register([FromForm] RegisterUserDataIn dataIn)
        {
            return Ok(_userService.Save(dataIn));
        }
        
        [HttpGet("{userId}")]
        public ActionResult GetUser(int userId)
        {
            return Ok(_userService.Get(userId));
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login([FromBody] LoginDataIn loginData)
        {
            //DateTime passwordLimit = DateTime.Today.AddDays(-90);

            ResponsePackage<string> retval;
            var user = this._userService.GetUserByEmailAndPass(loginData.Email, loginData.Password);

            if (user.TransferObject != null)
            {
                if (user.TransferObject.Status != UserStatus.Approved)
                {
                    var tempstring = "";
                    if (user.TransferObject.Role == Role.Customer)
                        tempstring = "pending. (please check your email for activation)";
                    else if (user.TransferObject.Role == Role.Saler && user.TransferObject.Status == UserStatus.Pending)
                        tempstring = "pending. (please wait for approve by support)";
                    else if (user.TransferObject.Role == Role.Saler && user.TransferObject.Status == UserStatus.Rejected)
                        tempstring = "rejected. (please conntact support for more details)";

                    return Ok(new ResponsePackage<string>(ResponseStatus.Error, "Account is corrent, but his/her status is " + tempstring));
                }

                string token = JwtManager.GetToken(user.TransferObject, 60);
                retval = new ResponsePackage<string>(token);
                return Ok(new ResponsePackage<string>()
                {
                    Status = ResponseStatus.Ok,
                    Message = "Success loging!",
                    TransferObject = token
                });
            }
            else
                return Ok(new ResponsePackage<string>(ResponseStatus.Error, "Wrong email or password!"));
        }

        [AllowAnonymous]
        [HttpGet("activate/{email}/{key}")]
        public ActionResult Activate(string email, string key)
        {
            ResponsePackage<string> retval;
            var user = this._userService.ActivateUser(email, key);

            retval = new ResponsePackage<string>("Success");

            return Ok(retval);
        }

        [HttpGet("approveOrRejectUser/{userId}/{rejectOrApprove}")]
        [AllowAnonymous]
        public ActionResult ApproveOrRejectUser(int userId, bool rejectOrApprove)
        {
            return Ok(_userService.ApproveOrRejectUser(userId,rejectOrApprove));
        }

        [HttpGet("delete/{userId}")]
        [AllowAnonymous]
        public ActionResult Delete(int userId)
        {
            return Ok(_userService.Delete(userId));
        }
        [HttpPost("getAll")]
        public ActionResult GetAll([FromBody]PaginationDataIn dataIn)
        {
            var temp = GetUserId();
            return Ok(_userService.GetAll(dataIn));
        }
        
        [HttpPost("getByIds")]
        [AllowAnonymous]
        public ActionResult GetByIds([FromBody] List<int> ids)
        {
            return Ok(_userService.GetByIds(ids));
        }

        [HttpPost("facebookLogin")]
        [AllowAnonymous]
        public async Task<ActionResult> FacebookLogin([FromBody] FacebookLoginDataIn dataIn)
        {
            var userFromDb = await _userService.RegisterOrLoginFacebookUser(await _facebookService.GetUserData(dataIn.FacebookLoginToken));
            string token = JwtManager.GetToken(userFromDb, 60);
            if(userFromDb==null)
                return Ok(new ResponsePackage<string>()
                {
                    Status = ResponseStatus.Error,
                    Message = "Error ocurred!",
                });
            else
                return Ok(new ResponsePackage<string>()
                {
                    Status = ResponseStatus.Ok,
                    Message = "Success loging!",
                    TransferObject = token
                });
        }


    }
}
