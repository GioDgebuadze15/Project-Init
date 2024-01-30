using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ProjectInit.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseApiController : Controller
{
    private IMediator? _mediator;
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();

    protected string? UserId =>
        HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
            .Select(c => c.Value)
            .FirstOrDefault();

    protected string? UserEmail =>
        HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Email)
            .Select(c => c.Value)
            .FirstOrDefault();
}