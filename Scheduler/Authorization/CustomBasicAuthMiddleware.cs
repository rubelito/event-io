using System;
using System.Net.Http.Headers;
using System.Text;
using Scheduler.Services;

namespace Scheduler.Authorization
{
    public class CustomBasicAuthMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomBasicAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IUserService userService)
        {
            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(context.Request.Headers["Authorization"]);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':', 2);
                var firstName = credentials[0];
                var lastName = credentials[1];

                context.Items["User"] = userService.Authenticate(firstName, lastName);
            }
            catch
            {

            }

            await _next(context);
        }
    }
}

