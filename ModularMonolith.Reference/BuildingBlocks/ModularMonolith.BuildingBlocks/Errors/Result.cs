namespace ModularMonolith.BuildingBlocks.Errors;

public sealed record Error(string Code, string Message);

public sealed class Result<T>
{
    private Result(T? value, Error? error, int statusCode) { Value = value; Error = error; StatusCode = statusCode; }
    public T? Value { get; }
    public Error? Error { get; }
    public int StatusCode { get; }
    public bool IsSuccess => Error is null;
    public static Result<T> Success(T value, int statusCode = 200) => new(value, null, statusCode);
    public static Result<T> Failure(string code, string message, int statusCode) => new(default, new Error(code, message), statusCode);
}
