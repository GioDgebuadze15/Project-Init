namespace ProjectInit.Application.Responses;

public class BaseResponse
{
    public string Message { get; }
    public bool Success { get; }


    protected BaseResponse(string message, bool success = default)
    {
        Message = message;
        Success = success;
    }

    public static BaseResponse Ok(string message) => new(message, true);
    public static BaseResponse<T> Ok<T>(string message, T value) => new(message, value);
    public static BaseResponse Fail(string message) => new(message);
    public static BaseResponse<T> Fail<T>(string message) => new(message);
}