using Microsoft.AspNetCore.Mvc;
using ProjectInit.Application.Features.FileFeatures.Commands.Create;
using ProjectInit.Application.Responses;

namespace ProjectInit.API.Controllers;

public class FileController : BaseApiController
{
    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreateFile request)
        => Ok(await Bus.InvokeAsync<BaseResponse<int>>(request));
}