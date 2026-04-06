namespace CookWizard.Domain.Entities;

public class CookWizardApiResult<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public string? Error { get; }
    public bool IsFailure => !IsSuccess;

    protected CookWizardApiResult(bool isSuccess, T? value, string? error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    public static CookWizardApiResult<T> Success(T value) => new(true, value, null);
    public static CookWizardApiResult<T> Failure(string error) => new(false, default, error);
}