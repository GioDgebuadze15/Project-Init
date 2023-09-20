using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProjectInit.Application.Constants;
using ProjectInit.Application.Exceptions;
using ProjectInit.Application.Responses;
using ProjectInit.Domain.Entities.Common;
using Transmogrify;

namespace ProjectInit.API.Middlewares;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;
    private readonly IServiceProvider _serviceProvider;

    public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger,IServiceProvider serviceProvider)
    {
        _next = next;
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            // Todo: I dont know exactly why I need this, thus I'm adding ITranslator to DI, so check this code
            using var scope = _serviceProvider.CreateScope();
            var iTranslator = scope.ServiceProvider.GetRequiredService<ITranslator>();
            
            var response = context.Response;
            response.ContentType = ProjectInitConstants.DefaultContentType;
            BaseResponse responseModel;

            switch (e)
            {
                case ArgumentNullException:
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    responseModel = BaseResponse.Fail(e.Message);
                    break;
                case FluentValidation.ValidationException:
                    response.StatusCode = StatusCodes.Status422UnprocessableEntity;
                    responseModel = BaseResponse.Fail(e.Message);
                    break;
                case EntityNotFoundException exception:
                    response.StatusCode = StatusCodes.Status404NotFound;
                    responseModel = BaseResponse.Fail(
                        await iTranslator
                            .GetTranslation(
                                LanguageConstants.ErrorsFileName,
                                response.StatusCode.ToString(),
                                exception.EntityType,
                                exception.EntityId
                            ));
                    break;
                case DbUpdateConcurrencyException:
                    response.StatusCode = StatusCodes.Status409Conflict;
                    responseModel = BaseResponse.Fail(
                        await iTranslator
                            .GetTranslation(
                                LanguageConstants.ErrorsFileName,
                                response.StatusCode.ToString()
                            ));
                    break;
                default:
                    response.StatusCode = StatusCodes.Status500InternalServerError;
                    responseModel = BaseResponse.Fail(
                        await iTranslator
                            .GetTranslation(
                                LanguageConstants.ErrorsFileName,
                                response.StatusCode.ToString()
                            ));
                    break;
            }

            var result = JsonConvert.SerializeObject(responseModel);
            await response.WriteAsync(result);
            _logger.LogError(e, LoggerConstants.LoggerMessage);
        }
    }
}