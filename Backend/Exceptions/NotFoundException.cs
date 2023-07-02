namespace Backend.Exceptions;

public class NotFoundException : Exception
{
    public string Messages { get; set; }

    public NotFoundException()
    {
        Messages = "Not Found";
    }

    public NotFoundException(string message)
    {
        Messages = message;
    }
}