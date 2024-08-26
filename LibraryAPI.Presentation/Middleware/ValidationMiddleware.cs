using LibraryAPI.Services.Exceptions;
using Microsoft.AspNetCore.Http;

namespace LibraryAPI.Presentation.Middleware
{
    public class ValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.HasFormContentType || !context.Request.Form.Any())
            {
                await _next(context);
                return;
            }

            var validationErrors = new Dictionary<string, string[]>();

            foreach (var field in context.Request.Form)
            {
                if (string.IsNullOrWhiteSpace(field.Value))
                {
                    validationErrors[field.Key] = new[] { "Field is required" };
                }
            }

            if (validationErrors.Any())
            {
                throw new ValidationException(validationErrors);
            }

            await _next(context);
        }
    }

}
