namespace CookWizard.Application.Common;

public class ResultObject<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public string? Error { get; }
    public bool IsFailure => !IsSuccess;

    protected ResultObject(bool isSuccess, T? value, string? error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    public static ResultObject<T> Success(T value) => new(true, value, null);
    public static ResultObject<T> Failure(string error) => new(false, default, error);
}