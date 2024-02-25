using ProjectInit.Shared.Constants;

namespace ProjectInit.Shared.Helpers;

public static class LanguageHelper
{
    public static string GetLanguageFolderFullPath()
        => Path.Combine(
            Directory.EnumerateDirectories(
                    Directory.GetParent(
                        Directory.GetCurrentDirectory()
                    )!.FullName)
                .SingleOrDefault(dir => dir.Contains(
                    ProjectInitConstants.SharedFolderName,
                    StringComparison.OrdinalIgnoreCase
                ))!,
            LanguageConstants.LanguageFolder
        );
}