namespace ContainerChallenge.Validation;

public class ValidationResult
{
    private ValidationResult(bool ok, string? error)
    {
        IsValid = ok;
        Error = error;
    }

    public bool IsValid { get; }
    public string? Error { get; }

    public static ValidationResult Ok() => new(true, null);
    public static ValidationResult Fail(string error) => new(false, error);
}
