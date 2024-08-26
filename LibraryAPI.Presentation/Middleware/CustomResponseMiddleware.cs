using Microsoft.AspNetCore.Http;

namespace LibraryAPI.Presentation.Middleware
{
    public class CustomResponseMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.OnStarting(() =>
            {
                if (context.Response.StatusCode >= 400)
                {
                    context.Response.ContentType = "application/json";
                }
                return Task.CompletedTask;
            });

            await _next(context);
        }
    }

}
