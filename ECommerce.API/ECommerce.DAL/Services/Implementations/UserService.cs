using ECommerce.DAL.Data;
using ECommerce.DAL.DTO;
using ECommerce.DAL.DTO.User.DataIn;
using ECommerce.DAL.Services.Interfaces;
using ECommerce.DAL.UOWs;
using ECommerce.DAL.Models;
using ECommerce.DAL.DTO.User.DataOut;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.IdentityModel.Tokens;
using ECommerce.DAL.DTO.Product.DataOut;
using Microsoft.Identity.Client;

namespace ECommerce.DAL.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly UserDbContext _dbContext;
        private readonly IEmailService _emailService;
        private readonly IUnitOfWorkUser _uowUser;
        private readonly IPasswordMD5Service _passwordMD5Service;
        public UserService(IUnitOfWorkUser uowUser, IEmailService userService, IPasswordMD5Service passwordMD5Service)
        {
            _uowUser = uowUser;
            _emailService = userService;
            _passwordMD5Service = passwordMD5Service;
        }

        public ResponsePackage<string> ActivateUser(string email, string key)
        {
            var userFromDb = _uowUser.GetUserRepository().GetUserByEmail(email);
            if(userFromDb.ActivateKey == key) 
                userFromDb.Status = UserStatus.Approved;
            _uowUser.Save();

            return new ResponsePackage<string>()
            {
                Status = 200,
                Message = "Successfully activated account! "
            };
        }

        public ResponsePackage<string> ApproveOrRejectUser(int userId, bool rejectOrApprove)
        {
            string ret = "";
            string emailContent = "";
            var tempUser = _uowUser.GetUserRepository().GetUserById(userId);

            if(tempUser == null)
                return new ResponsePackage<string>
                {
                    Message = "User doesn't exist in database!",
                    Status = 200
                };


            //action
            if (tempUser.Status == UserStatus.Pending)
            {
                tempUser.Status = rejectOrApprove ? UserStatus.Approved : UserStatus.Rejected;
                _uowUser.Save();
                ret = rejectOrApprove ? "User " + tempUser.FirstName + " " + tempUser.LastName + " is successful approved!" : "User " + tempUser.FirstName + " " + tempUser.LastName + " is successful rejected!";
                if (rejectOrApprove)
                {
                    #region emailContent 
                    var tempstring1 = @"<!DOCTYPE html>
        <html>
        <head>
            <title></title>
            <meta http-equiv='Content-Type' content='text/html; charset=utf-8' />
            <meta name='viewport' content='width=device-width, initial-scale=1'>
            <meta http-equiv='X-UA-Compatible' content='IE=edge' />
<style type='text/css'>
	
        @media screen {
            @font-face {
                font-family: 'Lato';
                font-style: normal;
                font-weight: 400;
                src: local('Lato Regular'), local('Lato-Regular'), url(https://fonts.gstatic.com/s/lato/v11/qIIYRU-oROkIk8vfvxw6QvesZW2xOQ-xsNqO47m55DA.woff) format('woff');
            }

            @font-face {
                font-family: 'Lato';
                font-style: normal;
                font-weight: 700;
                src: local('Lato Bold'), local('Lato-Bold'), url(https://fonts.gstatic.com/s/lato/v11/qdgUG4U09HnJwhYI-uK18wLUuEpTyoUstqEm5AMlJo4.woff) format('woff');
            }

            @font-face {
                font-family: 'Lato';
                font-style: italic;
                font-weight: 400;
                src: local('Lato Italic'), local('Lato-Italic'), url(https://fonts.gstatic.com/s/lato/v11/RYyZNoeFgb0l7W3Vu1aSWOvvDin1pK8aKteLpeZ5c0A.woff) format('woff');
            }

            @font-face {
                font-family: 'Lato';
                font-style: italic;
                font-weight: 700;
                src: local('Lato Bold Italic'), local('Lato-BoldItalic'), url(https://fonts.gstatic.com/s/lato/v11/HkF_qI1x_noxlxhrhMQYELO3LdcAZYWl9Si6vvxL-qU.woff) format('woff');
            }
        }

        /* CLIENT-SPECIFIC STYLES */
        body,
        table,
        td,
        a {
            -webkit-text-size-adjust: 100%;
            -ms-text-size-adjust: 100%;
        }

        table,
        td {
            mso-table-lspace: 0pt;
            mso-table-rspace: 0pt;
        }

        img {
            -ms-interpolation-mode: bicubic;
        }

        /* RESET STYLES */
        img {
            border: 0;
            height: auto;
            line-height: 100%;
            outline: none;
            text-decoration: none;
        }

        table {
            border-collapse: collapse !important;
        }
a{
text-decoration: none;
color: white;
text-transform: uppercase;
justify-content: center;
align: center;
}



        body {
            height: 100% !important;
            margin: 0 !important;
            padding: 0 !important;
            width: 100% !important;
        }

        /* iOS BLUE LINKS */
        a[x-apple-data-detectors] {
            color: inherit !important;
            text-decoration: none !important;
            font-size: inherit !important;
            font-family: inherit !important;
            font-weight: inherit !important;
            line-height: inherit !important;
        }

        /* MOBILE STYLES */
        @media screen and (max-width:600px) {
            h1 {
                font-size: 32px !important;
                line-height: 32px !important;
            }
        }

        /* ANDROID CENTER FIX */
        div[style*='margin: 16px 0;'] {
            margin: 0 !important;
        }
    </style>
        </head>
        
        <body style='background-color: #f4f4f4; margin: 0 !important; padding: 0 !important;'>
        <!-- HIDDEN PREHEADER TEXT -->
        <table border='0' cellpadding='0' cellspacing='0' width='100%'>
            <!-- LOGO -->
            <tr>
                <td bgcolor='#ff66ff' align='center'>
                    <table border='0' cellpadding='0' cellspacing='0' width='100%' style='max-width: 600px;'>
                        <tr>
                            <td align='center' valign='top' style='padding: 40px 10px 40px 10px;'> </td>
                        </tr>
                    </table>
                </td>
            </tr>
<tr>
        <td bgcolor='#ff66ff' align='center' style='padding: 0px 10px 0px 10px;'>
            <table border='0' cellpadding='0' cellspacing='0' width='100%' style='max-width: 600px;'>
                <tr>
                    <td bgcolor='#ffffff' align='center' valign='top' style='padding: 40px 20px 20px 20px; border-radius: 4px 4px 0px 0px; color: #111111; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 48px; font-weight: 400; letter-spacing: 4px; line-height: 48px;'>
                        <h1 style='font-size: 48px; font-weight: 400; margin: 2;'>ACCOUNT STATUS</h1> <img src='https://ibb.co/JpLLcsH' width='180' height=auto style='display: block; border: 0px;' />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td bgcolor='#f4f4f4' align='center' style='padding: 0px 10px 0px 10px;'>
            <table border='0' cellpadding='0' cellspacing='0' width='100%' style='max-width: 600px;'>
               <!-- <tr>
                    <td bgcolor='#ffffff' align='left' style='padding: 20px 30px 20px 30px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;'>
                        <p style='margin: 0; text-align:center'>YOUR OPT : *****</p>
                    </td>
                </tr>-->
                <tr>
                    <td bgcolor='#ffffff' align='left'>
                        <table width='100%' border='0' cellspacing='0' cellpadding='0'>
                            <tr>
                                <td bgcolor='#ffffff' align='center' style='padding: 20px 30px 30px 30px;'>
                                    <table border='0' cellspacing='0' cellpadding='0'>
                                        <tr>
                                            <a href='https://localhost:3000'><td align='center' style='border-radius: 3px;' bgcolor='#ff66ff'><a href='https://localhost:3000' target='_blank' style='font-size: 20px; font-family: Helvetica, Arial, sans-serif; color: #ffffff; text-decoration: none; color: #ffffff; text-decoration: none; padding: 15px 25px; border-radius: 2px; border: 1px solid #ff66ff; display: inline-block;'>";
                    var tempstring2 = @"</td></a>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr> <!-- COPY -->
                <tr>
                    <td bgcolor='#ffffff' align='left' style='padding: 0px 30px 20px 30px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;'>
                        
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td bgcolor='#f4f4f4' align='center' style='padding: 30px 10px 0px 10px;'>
            <table border='0' cellpadding='0' cellspacing='0' width='100%' style='max-width: 600px;'>
                <tr>
                    <td bgcolor='#ff0000' align='center' style='padding: 30px 30px 30px 30px; border-radius: 4px 4px 4px 4px; color: #fff; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;'>
                        <h2 style='font-size: 20px; font-weight: 400; color: #fff; margin: 0;'>Need more help?</h2>
                        <p style='margin: 0;'><a href='mailto:rastislavraki1@gmail.com' target='_blank' style='color: #fff;'>We&rsquo;re here to help you out</a></p>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
            </table>
        </body>
        </html>
        ";
                    var tempstring3 = rejectOrApprove ? "Approved" : "Rejected";
                    emailContent = tempstring1 + tempstring3 + tempstring2;
                    #endregion

                    _emailService.SendEmail(tempUser.Email, emailContent, "Boom Shop | ACCOUNT STATUS UPDATE");
                }

            }
            else
            {
                ret = "User " + tempUser.FirstName + " " + tempUser.LastName + " isn't in pending state!";
            }

            return new ResponsePackage<string>
            {
                Message = ret,
                Status = 200
            };
        }

        public ResponsePackage<string> Delete(int userId)
        {
            var tempUser = _uowUser.GetUserRepository().GetUserById(userId);
            if(tempUser == null)
                return new ResponsePackage<string>
                {
                    Message = "User doesn't exist in database!",
                    Status = 200
                };
            else
            {
                tempUser.IsDeleted = true;
                _uowUser.Save();
                return new ResponsePackage<string>
                {
                    Message = "User has been successfully deleted!",
                    Status = 200
                };
            }
        }

        public ResponsePackage<UserDataOut> Get(int userId)
        {
            var user = _uowUser.GetUserRepository().GetUserById(userId);
            if (user != null)
                return new ResponsePackage<UserDataOut>
                {
                    TransferObject = new UserDataOut()
                    {
                        Id = user.Id,
                        Address = user.Address,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        UserName = user.UserName,
                        BirthDate = user.BirthDate.GetValueOrDefault(),
                        Role = user.Role.ToString(),
                        Image = user.Image
                    },
                    Status = 200,
                };
            else return new ResponsePackage<UserDataOut>()
            {
                Status = 400,
                Message = "User doesn't exist in database."
            };
        }

        public ResponsePackage<PaginationDataOut<UserDataOut>> GetAll(PaginationDataIn dataIn)
        {
            var products = _uowUser.GetUserRepository().GetAllUsersWithPagination(dataIn);
            var data = products.TransferObject.Select(
                x=>new UserDataOut
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName= x.LastName,
                    Role = x.Role.ToString(),
                    Email = x.Email,
                    BirthDate = x.BirthDate.GetValueOrDefault(),
                    Address = x.Address,
                    Status = x.Status.ToString(),
                    Image = x.Image,
                    UserName = x.UserName,
                }).ToList();

            return new ResponsePackage<PaginationDataOut<UserDataOut>>()
            {
                Status = ResponseStatus.Ok,
                TransferObject = new PaginationDataOut<UserDataOut> { Data = data, Count = int.Parse(products.Message) }
            };
        }

        public List<UserDataOut> GetByIds(List<int> ids)
        {
            var users = _uowUser.GetUserRepository().GetUserByIds(ids);
            return users.Select(x => new UserDataOut(){
                FirstName = x.FirstName,
                LastName = x.LastName, 
                Id = x.Id,
                Address = x.Address,
                UserName = x.UserName,
                Email = x.Email
            }).ToList();
        }

        public ResponsePackage<User> GetUserByEmailAndPass(string email, string pass)
        {
            var userFromDb = _uowUser.GetUserRepository().GetUserByEmail(email);
            if (userFromDb == null || !_passwordMD5Service.VerifyMd5Hash(pass, userFromDb.Password))
                return new ResponsePackage<User>()
                {
                    Status = ResponseStatus.Unautorized,
                    Message = "Invalid email or password."
                };
            else
                return new ResponsePackage<User>
                {
                    Status = ResponseStatus.Ok,
                    TransferObject = userFromDb,
                };
        }

        public async Task<User> RegisterOrLoginFacebookUser(FacebookUserData dataIn)
        {
            var userFromDb = _uowUser.GetUserRepository().GetUserByEmail(dataIn.Email);
            if(userFromDb==null)
            {
                // ne postoji ajde da ga registrujem
                await _uowUser.GetUserRepository().AddAsync(new User()
                {
                    Email = dataIn.Email,
                    FirstName = dataIn.Name.Split(' ')[0],
                    LastName = dataIn.Name.Split(' ')[1],
                    Image = dataIn.Picture.Data.Url,
                    Role = Role.Customer,
                    LastUpdateTime = DateTime.Now,
                    Status = UserStatus.Approved,
                    Address = ""
                });
                await _uowUser.Save();
            }
            var newUser = _uowUser.GetUserRepository().GetUserByEmail(dataIn.Email);
            return newUser;
        }

        public async Task<ResponsePackage<string>> Save(RegisterUserDataIn dataIn)
        {

            dataIn.Email = dataIn.Email.ToLower().Trim();

            if(dataIn.Id == null) //create new
            {
                //generate user
                var userForDb = new User()
                {
                    Address = dataIn.Address,
                    Email = dataIn.Email,
                    FirstName = dataIn.FirstName,
                    UserName = dataIn.Username,
                    BirthDate = dataIn.BirthDate.GetValueOrDefault(),
                    LastName = dataIn.LastName,
                    Password = _passwordMD5Service.GetMd5Hash(dataIn.Password),
                    Role = (Role)Int32.Parse(dataIn.RoleId),
                    Status = ((Role)Int32.Parse(dataIn.RoleId)) == Role.Customer ? UserStatus.Approved : UserStatus.Pending,
                    ActivateKey = System.Guid.NewGuid().ToString(),
                    LastUpdateTime = DateTime.Now
                };

                if(dataIn.Image != null)
                    userForDb.Image = await userForDb.SaveImage(dataIn.Image);

                if (_uowUser.GetUserRepository().GetUserByEmail(userForDb.Email) != null)
                    return new ResponsePackage<string>(ResponseStatus.Error, "User with this email already exists.");
                
                if (_uowUser.GetUserRepository().GetUserByUserName(userForDb.UserName) != null)
                    return new ResponsePackage<string>(ResponseStatus.Error, "User with this username already exists.");

                _uowUser.GetUserRepository().AddAsync(userForDb);
                _uowUser.Save();

                #region EmailContent
                string htmlContent = @"
        <!DOCTYPE html>
        <html>
        <head>
            <title></title>
            <meta http-equiv='Content-Type' content='text/html; charset=utf-8' />
            <meta name='viewport' content='width=device-width, initial-scale=1'>
            <meta http-equiv='X-UA-Compatible' content='IE=edge' />
<style type='text/css'>
	
        @media screen {
            @font-face {
                font-family: 'Lato';
                font-style: normal;
                font-weight: 400;
                src: local('Lato Regular'), local('Lato-Regular'), url(https://fonts.gstatic.com/s/lato/v11/qIIYRU-oROkIk8vfvxw6QvesZW2xOQ-xsNqO47m55DA.woff) format('woff');
            }

            @font-face {
                font-family: 'Lato';
                font-style: normal;
                font-weight: 700;
                src: local('Lato Bold'), local('Lato-Bold'), url(https://fonts.gstatic.com/s/lato/v11/qdgUG4U09HnJwhYI-uK18wLUuEpTyoUstqEm5AMlJo4.woff) format('woff');
            }

            @font-face {
                font-family: 'Lato';
                font-style: italic;
                font-weight: 400;
                src: local('Lato Italic'), local('Lato-Italic'), url(https://fonts.gstatic.com/s/lato/v11/RYyZNoeFgb0l7W3Vu1aSWOvvDin1pK8aKteLpeZ5c0A.woff) format('woff');
            }

            @font-face {
                font-family: 'Lato';
                font-style: italic;
                font-weight: 700;
                src: local('Lato Bold Italic'), local('Lato-BoldItalic'), url(https://fonts.gstatic.com/s/lato/v11/HkF_qI1x_noxlxhrhMQYELO3LdcAZYWl9Si6vvxL-qU.woff) format('woff');
            }
        }

        /* CLIENT-SPECIFIC STYLES */
        body,
        table,
        td,
        a {
            -webkit-text-size-adjust: 100%;
            -ms-text-size-adjust: 100%;
        }

        table,
        td {
            mso-table-lspace: 0pt;
            mso-table-rspace: 0pt;
        }

        img {
            -ms-interpolation-mode: bicubic;
        }

        /* RESET STYLES */
        img {
            border: 0;
            height: auto;
            line-height: 100%;
            outline: none;
            text-decoration: none;
        }

        table {
            border-collapse: collapse !important;
        }
a{
text-decoration: none;
color: white;
text-transform: uppercase;
justify-content: center;
align: center;
}



        body {
            height: 100% !important;
            margin: 0 !important;
            padding: 0 !important;
            width: 100% !important;
        }

        /* iOS BLUE LINKS */
        a[x-apple-data-detectors] {
            color: inherit !important;
            text-decoration: none !important;
            font-size: inherit !important;
            font-family: inherit !important;
            font-weight: inherit !important;
            line-height: inherit !important;
        }

        /* MOBILE STYLES */
        @media screen and (max-width:600px) {
            h1 {
                font-size: 32px !important;
                line-height: 32px !important;
            }
        }

        /* ANDROID CENTER FIX */
        div[style*='margin: 16px 0;'] {
            margin: 0 !important;
        }
    </style>
        </head>
        
        <body style='background-color: #f4f4f4; margin: 0 !important; padding: 0 !important;'>
        <!-- HIDDEN PREHEADER TEXT -->
        <table border='0' cellpadding='0' cellspacing='0' width='100%'>
            <!-- LOGO -->
            <tr>
                <td bgcolor='#ff66ff' align='center'>
                    <table border='0' cellpadding='0' cellspacing='0' width='100%' style='max-width: 600px;'>
                        <tr>
                            <td align='center' valign='top' style='padding: 40px 10px 40px 10px;'> </td>
                        </tr>
                    </table>
                </td>
            </tr>
<tr>
        <td bgcolor='#ff66ff' align='center' style='padding: 0px 10px 0px 10px;'>
            <table border='0' cellpadding='0' cellspacing='0' width='100%' style='max-width: 600px;'>
                <tr>
                    <td bgcolor='#ffffff' align='center' valign='top' style='padding: 40px 20px 20px 20px; border-radius: 4px 4px 0px 0px; color: #111111; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 48px; font-weight: 400; letter-spacing: 4px; line-height: 48px;'>
                        <h1 style='font-size: 48px; font-weight: 400; margin: 2;'>Welcome!</h1> <img src='https://ibb.co/JpLLcsH' width='180' height=auto style='display: block; border: 0px;' />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td bgcolor='#f4f4f4' align='center' style='padding: 0px 10px 0px 10px;'>
            <table border='0' cellpadding='0' cellspacing='0' width='100%' style='max-width: 600px;'>
               <!-- <tr>
                    <td bgcolor='#ffffff' align='left' style='padding: 20px 30px 20px 30px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;'>
                        <p style='margin: 0; text-align:center'>YOUR OPT : *****</p>
                    </td>
                </tr>-->
                <tr>
                    <td bgcolor='#ffffff' align='left'>
                        <table width='100%' border='0' cellspacing='0' cellpadding='0'>
                            <tr>
                                <td bgcolor='#ffffff' align='center' style='padding: 20px 30px 30px 30px;'>
                                    <table border='0' cellspacing='0' cellpadding='0'>
                                        <tr>
                                            <a href='https://localhost:63290/api/User/activate/"+dataIn.Email+"/"+userForDb.ActivateKey+ @"'><td align='center' style='border-radius: 3px;' bgcolor='#ff66ff'><a href='https://localhost:63290/api/User/activate/"+dataIn.Email+"/"+userForDb.ActivateKey+ @"' target='_blank' style='font-size: 20px; font-family: Helvetica, Arial, sans-serif; color: #ffffff; text-decoration: none; color: #ffffff; text-decoration: none; padding: 15px 25px; border-radius: 2px; border: 1px solid #ff66ff; display: inline-block;'>Activate Account</td></a>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr> <!-- COPY -->
                <tr>
                    <td bgcolor='#ffffff' align='left' style='padding: 0px 30px 20px 30px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;'>
                        <p style='margin: 0;'>If you have any questions, just reply to this email&mdash;we're always happy to help out.</p>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td bgcolor='#f4f4f4' align='center' style='padding: 30px 10px 0px 10px;'>
            <table border='0' cellpadding='0' cellspacing='0' width='100%' style='max-width: 600px;'>
                <tr>
                    <td bgcolor='#ff0000' align='center' style='padding: 30px 30px 30px 30px; border-radius: 4px 4px 4px 4px; color: #fff; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;'>
                        <h2 style='font-size: 20px; font-weight: 400; color: #fff; margin: 0;'>Need more help?</h2>
                        <p style='margin: 0;'><a href='mailto:rastislavraki1@gmail.com' target='_blank' style='color: #fff;'>We&rsquo;re here to help you out</a></p>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
            </table>
        </body>
        </html>
        ";
                #endregion

                if(userForDb.Role == Role.Customer)
                    _emailService.SendEmail(dataIn.Email, htmlContent, "Activate account on Boom Shop");

                return new ResponsePackage<string>(ResponseStatus.Ok, "Successfully register new user, please check you Email.");
            }
            else // edit exist
            {
                var dbUser = _uowUser.GetUserRepository().GetUserById(dataIn.Id.GetValueOrDefault());
                if(dbUser == null)
                    return new ResponsePackage<string>(ResponseStatus.Error, "User doesn't exist in database.");

                string tempImage = "";
                if (dataIn.Image != null)
                    tempImage = await dbUser.SaveImage(dataIn.Image);

                if (!string.IsNullOrEmpty(dataIn.Address) && dataIn.Address != dbUser.Address)
                    dbUser.Address = dataIn.Address;
                if (!string.IsNullOrEmpty(dataIn.Email) && dataIn.Email != dbUser.Email)
                    dbUser.Email = dataIn.Email;
                if (!string.IsNullOrEmpty(dataIn.FirstName) && dataIn.FirstName != dbUser.FirstName)
                    dbUser.FirstName = dataIn.FirstName;
                if (!string.IsNullOrEmpty(dataIn.LastName) && dataIn.LastName != dbUser.LastName)
                    dbUser.LastName = dataIn.LastName;
                if (!string.IsNullOrEmpty(tempImage) && tempImage != dbUser.Image)
                    dbUser.Image = tempImage;
                if (!string.IsNullOrEmpty(dataIn.Password) && dataIn.Password != dbUser.Password)
                    dbUser.Password = _passwordMD5Service.GetMd5Hash(dataIn.Password);
                //if (!string.IsNullOrEmpty(dataIn.RoleId) && dataIn.RoleId != "undefined" && ((Role)Int32.Parse(dataIn.RoleId)) != dbUser.Role && dataIn.RoleId != null)
                    //dbUser.Role = (Role)Int32.Parse(dataIn.RoleId);
                if (dataIn.BirthDate != null && dataIn.BirthDate != dbUser.BirthDate)
                    dbUser.BirthDate = dataIn.BirthDate.GetValueOrDefault();
                dbUser.LastUpdateTime = DateTime.Now;

                _uowUser.Save();
                return new ResponsePackage<string>(ResponseStatus.Ok, "Successfully edited user.");
            }
        }
    }
}
