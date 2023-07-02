using System.Collections;

namespace Backend.Exceptions;

public class FormValidationException : Exception
{
    public string? ErrorMessages { get; set; }
    public List<Dictionary<string, string>>? Errors { get; set; }

    public FormValidationException(string message)
    {
        ErrorMessages = message;
    }

    public FormValidationException(List<Dictionary<string, string>> errors)
    {
        Errors = errors;
    }
}