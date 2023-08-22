namespace ProjectInit.Application.Responses;

public class BaseResponse<T> : BaseResponse
{
    public T Value { get; } = default!;

    public BaseResponse(string message)
        :base(message){}

    public BaseResponse(string message, T value)
        : base(message, true)
    {
        Value = value;
    }
}