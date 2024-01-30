namespace ProjectInit.Application.Responses;

public class BaseResponse<T> : BaseResponse
    where T : notnull
{
    public T? Value { get; }

    public BaseResponse(string message)
        : base(message)
    {
    }

    public BaseResponse(string message, T value)
        : base(message, true)
    {
        Value = value;
    }
}