namespace ProjectInit.Shared.Settings;

public sealed record WebDavSettings(
    string BaseAddress,
    string StaticUrl,
    string Username,
    string Password
);