using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using WebAppRegistration.Data;
using WebAppRegistration.Models;
using System;

namespace WebAppRegistration.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ApplicationDbContext dbContext)
        {
            await _next(context);


            var log = new RequestLog
            {
                Timestamp = DateTime.UtcNow,
                Path = context.Request.Path,
                UserLogin = context.User.Identity.IsAuthenticated
                    ? context.User.Identity.Name
                    : "Anonymous",
                StatusCode = context.Response.StatusCode
            };

            dbContext.RequestLogs.Add(log);
            await dbContext.SaveChangesAsync();
        }
    }
}