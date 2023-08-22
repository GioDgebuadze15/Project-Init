using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ProjectInit.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseApiController: Controller
{
    private readonly Lazy<IMediator> _mediatorLazy;

    protected IMediator Mediator => _mediatorLazy.Value;

    public BaseApiController()
    {
        _mediatorLazy = new Lazy<IMediator>(() => HttpContext.RequestServices.GetService<IMediator>() ?? throw new InvalidOperationException());
    }

    protected string? UserId =>
        HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
            .Select(c => c.Value)
            .FirstOrDefault();

    protected string? UserEmail =>
        HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Email)
            .Select(c => c.Value)
            .FirstOrDefault();
}