using Microsoft.AspNetCore.Http;
using ProjectInit.Application.Constants;
using Transmogrify;

namespace ProjectInit.Application.Services.Language;

public class DefaultLanguageResolver(IHttpContextAccessor httpContextAccessor) : ILanguageResolver
{
    public Task<string> GetLanguageCode()
    {
        if (httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(LanguageConstants.LanguageKey, out var lang))
        {
            if (!string.IsNullOrWhiteSpace(lang))
            {
                return Task.FromResult(lang.ToLower());
            }
        }

        lang = GetLanguage();

        httpContextAccessor.HttpContext.Response.Cookies.Append(LanguageConstants.LanguageKey, lang);
        return Task.FromResult(lang.ToLower());
    }


    private string GetLanguage()
    {
        if (httpContextAccessor.HttpContext.Request.Headers.TryGetValue(LanguageConstants.LanguageHeadersKey,
                out var lang))
            return lang.First();

        return httpContextAccessor.HttpContext.Request.Query.TryGetValue(LanguageConstants.LanguageKey, out lang)
            ? lang.First()
            : LanguageConstants.EmptyLanguageCode;
    }
}