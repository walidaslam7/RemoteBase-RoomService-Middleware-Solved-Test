using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace RoomService.WebAPI
{
    public class PasswordCheckerMiddleware
    {
        private readonly RequestDelegate _next;
        public PasswordCheckerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.Values.Contains("passwordKey123456789"))
            {
                context.Response.StatusCode = 403;
                return;
            }
            await _next(context);
        }
    }
}
