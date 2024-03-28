namespace ProjectInit.Domain.Entities.Common;

public sealed record Error(Exception Exception)
{
    public int Id;
}