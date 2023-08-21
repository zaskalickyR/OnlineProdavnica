//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc.Filters;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace ECommerce.DAL.Helpers
//{
//    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
//    public class AuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
//    {
//        private readonly string[] _roles;

//        public AuthorizeAttribute(string[] role)
//        {
//            _roles = role;
//        }

//        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
//        {
//            // skip authorization if action is decorated with [AllowAnonymous] attribute
//            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
//            if (allowAnonymous)
//                return;

//            var idClaim = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "role")?.Value;
//            if (idClaim == null)
//            {
//                // not logged in
//                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
//            }
//            else
//            {
//                bool response = false;

//                if (!_roles.Contains(idClaim))
//                    context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
//            }
//        }
//    }
//}
