using Marten;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProjectInit.Application.Responses;
using ProjectInit.Domain.Entities.Common;
using ProjectInit.Shared.Constants;
using ProjectInit.Shared.Exceptions;
using Transmogrify;

namespace ProjectInit.API.Middlewares;

public class ErrorHandlerMiddleware(
    RequestDelegate next,
    ILogger<ErrorHandlerMiddleware> logger,
    IServiceProvider serviceProvider)
{
    private readonly ILogger _logger = logger;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            // Todo: I dont know exactly why I need this, thus I'm adding ITranslator to DI, so check this code
            using var scope = serviceProvider.CreateScope();
            var iTranslator = scope.ServiceProvider.GetRequiredService<ITranslator>();
            var iDocumentSession = scope.ServiceProvider.GetRequiredService<IDocumentSession>();

            var response = context.Response;
            response.ContentType = ApiConstants.DefaultContentType;
            BaseResponse responseModel;

            switch (e)
            {
                case ArgumentNullException:
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    responseModel = BaseResponse.Fail(e.Message);
                    break;
                case FluentValidationException exception:
                    response.StatusCode = StatusCodes.Status422UnprocessableEntity;
                    responseModel = BaseResponse.Fail(exception.Message, exception.ValidationErrors);
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

            iDocumentSession.Store(new Error(e));
            await iDocumentSession.SaveChangesAsync();
        }
    }
}