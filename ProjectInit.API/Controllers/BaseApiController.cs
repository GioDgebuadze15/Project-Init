using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace ProjectInit.API.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class BaseApiController : Controller
{
    private IMessageBus? _bus;
    protected IMessageBus Bus => _bus ??= HttpContext.RequestServices.GetRequiredService<IMessageBus>();

    protected string? UserId =>
        HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
            .Select(c => c.Value)
            .FirstOrDefault();

    protected string? UserEmail =>
        HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Email)
            .Select(c => c.Value)
            .FirstOrDefault();
}