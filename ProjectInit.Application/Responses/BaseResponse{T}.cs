namespace ProjectInit.Application.Responses;

public class BaseResponse<T> : BaseResponse
    where T : notnull
{
    public T? Data { get; }
    public T? Errors { get; }

    public BaseResponse(string message)
        : base(message)
    {
    }

    public BaseResponse(string message, T data)
        : base(message, true)
    {
        Data = data;
    }

    public BaseResponse(string message, T errors, bool success)
        : base(message, success)
    {
        Errors = errors;
    }
}