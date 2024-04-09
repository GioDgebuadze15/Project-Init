namespace ProjectInit.Application.Responses;

public class BaseResponse<T> : BaseResponse
    where T : notnull
{
    public T? Data { get; }
    public T? Errors { get; }

    public BaseResponse(string message, T data, bool success)
        : base(message, success)
    {
        if (success) Data = data;
        else Errors = data;
    }
}