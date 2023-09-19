using Microsoft.AspNetCore.Mvc;
using ProjectInit.Application.Responses;
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
        // throw new Exception("sda");
        var error =  await _translator.GetTranslation("Errors", "Hello");
        return Ok(BaseResponse.Fail(error));
    }
}