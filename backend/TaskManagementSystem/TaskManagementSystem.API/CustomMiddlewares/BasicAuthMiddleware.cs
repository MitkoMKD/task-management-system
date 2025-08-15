using Microsoft.EntityFrameworkCore;
using System.Text;
using TaskManagementSystem.Data;

namespace TaskManagementSystem.API.CustomMiddlewares {
    public class BasicAuthMiddleware {
        private readonly RequestDelegate _next;

        public BasicAuthMiddleware(RequestDelegate next) {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context) {
            string authHeader = context.Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Basic ")) {
                string encodedCredentials = authHeader.Substring("Basic ".Length).Trim();
                string credentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));
                string[] parts = credentials.Split(':', 2);

                if (parts.Length == 2) {
                    string username = parts[0];
                    string password = parts[1];

                    var dbContext = context.RequestServices.GetRequiredService<TaskContext>();
                    var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);

                    if (user != null && VerifyPassword(password, user.PasswordHash)) // Implement hashing
                    {
                        await _next(context);
                        return;
                    }
                }
            }

            context.Response.StatusCode = 401;
            context.Response.Headers["WWW-Authenticate"] = "Basic realm=\"Task Management System\"";
            await context.Response.WriteAsync("Unauthorized");
        }

        private bool VerifyPassword(string password, string passwordHash) {
            // For development, use plain text comparison (replace with hashing in production)
            return password == passwordHash; // Use a hashing library (e.g., BCrypt) in production
        }
    }

    public static class BasicAuthMiddlewareExtensions {
        public static IApplicationBuilder UseBasicAuth(this IApplicationBuilder builder) {
            return builder.UseMiddleware<BasicAuthMiddleware>();
        }
    }
}
