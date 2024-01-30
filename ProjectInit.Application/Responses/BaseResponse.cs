namespace ProjectInit.Application.Responses;

public class BaseResponse
{
    protected BaseResponse(string message, bool success = default)
    {
        Message = message;
        Success = success;
    }

    public string Message { get; }
    public bool Success { get; }

    public static BaseResponse Ok(string message) => new(message, true);
    public static BaseResponse<T> Ok<T>(string message, T value) where T : notnull => new(message, value);
    public static BaseResponse Fail(string message) => new(message);
    public static BaseResponse<T> Fail<T>(string message) where T : notnull => new(message);
}