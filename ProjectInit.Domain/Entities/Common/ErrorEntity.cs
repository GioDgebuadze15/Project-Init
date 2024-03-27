namespace ProjectInit.Domain.Entities.Common;

public class ErrorEntity(Exception exception) : BaseEntity<int>
{
    public Exception Exception { get; private set; } = exception;
}