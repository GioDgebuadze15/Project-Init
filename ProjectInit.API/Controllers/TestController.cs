using Microsoft.AspNetCore.Mvc;
using ProjectInit.Application.Responses;
using ProjectInit.Domain.Entities.Common;
using ProjectInit.Shared.Exceptions;
using Transmogrify;

namespace ProjectInit.API.Controllers;


public class TestController:BaseApiController
{
    private readonly ITranslator _translator;

    public TestController(ITranslator translator)
    {
        _translator = translator;
    }
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        throw new EntityNotFoundException<BaseEntity,string>("50");
        var error =  await _translator.GetTranslation("Errors", "Hello");
        return Ok(BaseResponse.Fail(error));
    }
}