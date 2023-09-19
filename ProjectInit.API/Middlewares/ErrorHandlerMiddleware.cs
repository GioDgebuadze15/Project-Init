using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProjectInit.Application.Responses;

namespace ProjectInit.API.Middlewares;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            var responseModel = BaseResponse.Fail(e.Message);

            response.StatusCode = e switch
            {
                ValidationException => StatusCodes.Status400BadRequest,
                FluentValidation.ValidationException => StatusCodes.Status422UnprocessableEntity,
                KeyNotFoundException => StatusCodes.Status404NotFound,
                DbUpdateConcurrencyException => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            };
            
            var result = JsonConvert.SerializeObject(responseModel);
            await response.WriteAsync(result);
            _logger.LogError(e, "Api threw an exception");
        }
    }
}