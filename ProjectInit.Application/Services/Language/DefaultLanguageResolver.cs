using Microsoft.AspNetCore.Http;
using Transmogrify;

namespace ProjectInit.Application.Services.Language;

public class DefaultLanguageResolver : ILanguageResolver
{
    private readonly HttpContext _httpContext;

    public DefaultLanguageResolver(IHttpContextAccessor httpContextAccessor)
    {
        _httpContext = httpContextAccessor.HttpContext;
    }

    public Task<string> GetLanguageCode()
    {
        if (_httpContext.Request.Cookies.TryGetValue("lang", out var lang))
        {
            if (!string.IsNullOrWhiteSpace(lang))
            {
                return Task.FromResult(lang.ToLower());
            }
        }

        lang = GetLanguage();

        _httpContext.Response.Cookies.Append("lang", lang);
        return Task.FromResult(lang.ToLower());
    }


    private string GetLanguage()
    {
        if (_httpContext.Request.Headers.TryGetValue("Accept-Language", out var lang))
            return lang.First();

        return _httpContext.Request.Query.TryGetValue("lang", out lang)
            ? lang.First()
            : "";
    }
}