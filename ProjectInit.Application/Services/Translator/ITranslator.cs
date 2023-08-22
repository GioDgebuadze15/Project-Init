namespace ProjectInit.Application.Services.Translator;

public interface ITranslator
{
    Task<string> GetTranslation(string fileName, string key, string? languagesFolderName = null);
}