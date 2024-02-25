namespace ProjectInit.Shared.Constants;

public abstract class DatabaseConstants
{
    public const string PostgreConnectionName = "DefaultDb";
    public const string InMemoryDatabaseName = "DefaultDb";
    public const string PostgreBehavior = "Npgsql.EnableLegacyTimestampBehavior";
}