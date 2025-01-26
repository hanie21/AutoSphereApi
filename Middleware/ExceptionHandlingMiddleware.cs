using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace AutoSphere.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Proceed with the next middleware in the pipeline
                await _next(context);
            }
            catch (Exception ex)
            {
                // Handle exception and return a response
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            int statusCode;
            string message;

            if (exception is UnauthorizedAccessException)
            {
                statusCode = StatusCodes.Status401Unauthorized;
                message = "Unauthorized access. Please check your credentials.";
            }
            else if (exception is KeyNotFoundException)
            {
                statusCode = StatusCodes.Status404NotFound;
                message = "The requested resource was not found.";
            }
            else if (exception is ArgumentException || exception is ArgumentNullException)
            {
                statusCode = StatusCodes.Status400BadRequest;
                message = "Invalid request. Please check the input.";
            }
            else
            {
                // Default to 500 Internal Server Error for unhandled exceptions
                statusCode = StatusCodes.Status500InternalServerError;
                message = "An internal server error occurred.";
            }

            // Create the response object
            var response = new
            {
                Message = message,
                Detail = exception.Message,
                StatusCode = statusCode
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}