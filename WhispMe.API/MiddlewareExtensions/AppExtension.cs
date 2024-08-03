using WhispMe.API.Middlewares;

namespace WhispMe.API.MiddlewareExtensions;

public static class AppExtension
{
    public static void ConfigureExceptionHandling(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
    }

    public static void ConfigureExceptionLogging(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionLoggingMiddleware>();
    }

    public static void ConfigureRequestLoggingMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<RequestLoggingMiddleware>();
    }
}
