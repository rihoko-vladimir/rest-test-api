using System.Net;

namespace TestTaskApi.Models;

public struct Result<T>
{
    public T? Value { get; }
    public Exception? Exception { get; }

    public HttpStatusCode StatusCode { get; } = HttpStatusCode.OK;

    public string Message { get; }
    public bool IsSuccess { get; }

    public Result(T value)
    {
        IsSuccess = true;
        Exception = null;
        Value = value;
        Message = string.Empty;
    }

    public Result(string message, HttpStatusCode statusCode)
    {
        IsSuccess = false;
        Exception = null;
        Value = default;
        StatusCode = statusCode;
        Message = message;
    }

    public Result(Exception exception, HttpStatusCode statusCode)
    {
        Exception = exception ?? throw new ArgumentNullException(nameof(exception));
        IsSuccess = false;
        Value = default;
        StatusCode = statusCode;
        Message = exception.Message;
    }
}

public struct Result
{
    public Exception? Exception { get; }

    public HttpStatusCode StatusCode { get; } = HttpStatusCode.OK;

    public string Message { get; }
    public bool IsSuccess { get; }

    public Result(bool success)
    {
        IsSuccess = success;
        StatusCode = HttpStatusCode.OK;
        Exception = null;
        Message = string.Empty;
    }

    public Result(Exception exception, HttpStatusCode statusCode)
    {
        Exception = exception ?? throw new ArgumentNullException(nameof(exception));
        StatusCode = statusCode;
        IsSuccess = false;
        Message = exception.Message;
    }

    public Result(string message, HttpStatusCode statusCode)
    {
        IsSuccess = false;
        Exception = null;
        Message = message;
        StatusCode = statusCode;
    }

    public static Result Success()
    {
        return new Result(true);
    }

    public static Result Error()
    {
        return new Result(false);
    }

    public static Result Error(Exception exception, HttpStatusCode statusCode)
    {
        return new Result(exception, statusCode);
    }

    public static Result Error(string message, HttpStatusCode statusCode)
    {
        return new Result(message, statusCode);
    }

    public static Result<T> Success<T>(T value)
    {
        return new Result<T>(value);
    }

    public static Result<T> Error<T>(Exception exception, HttpStatusCode statusCode)
    {
        return new Result<T>(exception, statusCode);
    }

    public static Result<T> Error<T>(string message, HttpStatusCode statusCode)
    {
        return new Result<T>(message, statusCode);
    }
}