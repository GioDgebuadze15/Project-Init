using Microsoft.AspNetCore.Http;
using ProjectInit.Application.Constants;
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
        if (_httpContext.Request.Cookies.TryGetValue(LanguageConstants.LanguageKey, out var lang))
        {
            if (!string.IsNullOrWhiteSpace(lang))
            {
                return Task.FromResult(lang.ToLower());
            }
        }

        lang = GetLanguage();

        _httpContext.Response.Cookies.Append(LanguageConstants.LanguageKey, lang);
        return Task.FromResult(lang.ToLower());
    }


    private string GetLanguage()
    {
        if (_httpContext.Request.Headers.TryGetValue(LanguageConstants.LanguageHeadersKey, out var lang))
            return lang.First();

        return _httpContext.Request.Query.TryGetValue(LanguageConstants.LanguageKey, out lang)
            ? lang.First()
            : LanguageConstants.EmptyLanguageCode;
    }
}