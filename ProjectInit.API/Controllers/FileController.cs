using Microsoft.AspNetCore.Mvc;
using ProjectInit.Application.Features.FileFeatures.Commands.Create;

namespace ProjectInit.API.Controllers;

public class FileController : BaseApiController
{
    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreateFileCommand request)
        => Ok(await Mediator.Send(request));
}