namespace ReelWords.Core;

public class InputResult
{
    public InputResult(bool isValid, string message)
    {
        Message = message;
        IsValid = isValid;
    }

    public bool IsValid { get; }
    public string Message { get; }
}