﻿namespace WhispMe.API.Middlewares;
public class ExceptionLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionLoggingMiddleware> _logger;

    public ExceptionLoggingMiddleware(RequestDelegate next, ILogger<ExceptionLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            LogExceptionDetails(ex);

            throw;
        }
    }

    private void LogExceptionDetails(Exception exception)
    {
        _logger.LogError("Exception: {Message}\nStack Trace: {StackTrace}", exception.Message, exception.StackTrace);
    }
}

