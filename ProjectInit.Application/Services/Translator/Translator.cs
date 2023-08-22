using Newtonsoft.Json;
using Transmogrify;

namespace ProjectInit.Application.Services.Translator;

public class Translator : ITranslator
{
    private const string LanguagesFolderName = "languages";
    private const string JsonExtension = ".json";
    private readonly ILanguageResolver _languageResolver;

    public Translator(ILanguageResolver languageResolver)
    {
        _languageResolver = languageResolver;
    }


    public async Task<string> GetTranslation(string fileName, string key, string? languagesFolderName = null)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentNullException(nameof(fileName));
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentNullException(nameof(key));

        var rootFolder = GetSolutionFolder();

        var languageFolders = FindFolders(rootFolder, languagesFolderName?.ToLower() ?? LanguagesFolderName).ToList();

        if (!languageFolders.Any()) throw new InvalidOperationException("No language folders found in the solution.");

        foreach (var folder in languageFolders)
        {
            var defaultLanguageCode = (await _languageResolver.GetLanguageCode())!;
            var filePath = Path.Combine(folder, defaultLanguageCode,$"{fileName}{JsonExtension}");
            if (!File.Exists(filePath)) continue;

            var json = File.ReadAllTextAsync(filePath).GetAwaiter().GetResult();
            var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            data!.TryGetValue(key, out var value);
            if (!string.IsNullOrWhiteSpace(value))
            {
                return value;
            }
        }

        throw new InvalidOperationException("Can't find translation.");
    }

    private static string GetSolutionFolder()
    {
        var currentDirectory = Directory.GetCurrentDirectory();

        while (!string.IsNullOrWhiteSpace(currentDirectory))
        {
            var solutionFiles = Directory.GetFiles(currentDirectory, "*.sln");

            if (solutionFiles.Length > 0)
            {
                return Path.GetDirectoryName(solutionFiles[0])!;
            }

            currentDirectory = Directory.GetParent(currentDirectory)?.FullName;
        }

        throw new ApplicationException("Solution folder not found.");
    }


    private static IEnumerable<string> FindFolders(string rootPath, string targetFolderName)
        =>
            Directory.GetDirectories(
                rootPath,
                targetFolderName,
                new EnumerationOptions
                {
                    MatchCasing = MatchCasing.CaseInsensitive,
                    RecurseSubdirectories = true,
                    AttributesToSkip = FileAttributes.System
                });
}