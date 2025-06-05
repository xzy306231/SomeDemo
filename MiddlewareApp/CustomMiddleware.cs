
namespace MiddlewareApp
{
    public class CustomMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await context.Response.WriteAsync("<!DOCTYPE html>");
            await context.Response.WriteAsync("<html lang=\"en\">");
            await context.Response.WriteAsync("<head>");
            await context.Response.WriteAsync("<meta charset=\"utf-8\">");
            await context.Response.WriteAsync("<title>My HTML Page</title>");
            await context.Response.WriteAsync("</head>");
            await context.Response.WriteAsync("<body>");
            //await context.Response.WriteAsync("<h1>Hello, World!</h1>");
            await context.Response.WriteAsync("</body>");
            await context.Response.WriteAsync("</html>");
            await next.Invoke(context);
        }
    }
}
