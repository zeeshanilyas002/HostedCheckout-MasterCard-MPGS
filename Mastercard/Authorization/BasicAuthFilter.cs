using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using GlobalPay.HostedCheckouts.Mastercard.Models;
using Microsoft.Extensions.Options;
namespace GlobalPay.HostedCheckouts.Mastercard.Authorization
{

    public class BasicAuthFilter : IAsyncAuthorizationFilter
    {
        private const string Realm = "TransferFloozApi";
        private readonly BasicAuth _authCreds;
        public BasicAuthFilter(IOptions<BasicAuth> authCreds)
        {
            _authCreds = authCreds.Value;
        }
        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor != null)
            {
                var actionAtt = controllerActionDescriptor.MethodInfo.GetCustomAttributes(inherit: false);
                if (actionAtt.OfType<AllowAnonymousAttribute>().Any())
                {
                    return Task.CompletedTask;
                }
            }


            string authHeader = context.HttpContext.Request.Headers["Authorization"];
            if (authHeader != null && authHeader.StartsWith("Basic "))
            {
                var encodedUsernamePassword = authHeader.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries)[1]?.Trim();
                var decodedUsernamePassword = Encoding.UTF8.GetString(Convert.FromBase64String(encodedUsernamePassword));
                var username = decodedUsernamePassword.Split(':', 2)[0];
                var password = decodedUsernamePassword.Split(':', 2)[1];

                if (IsAuthorized(username, password))
                {
                    return Task.CompletedTask;
                }
            }

            context.HttpContext.Response.Headers["WWW-Authenticate"] = $"Basic realm=\"{Realm}\", charset=\"UTF-8\"";
            context.Result = new UnauthorizedResult();
            return Task.CompletedTask;
        }

        public bool IsAuthorized(string username, string password)
        {
            return username.Equals(_authCreds.UserName, StringComparison.InvariantCultureIgnoreCase) && password == _authCreds.Password;
        }
    }

}
