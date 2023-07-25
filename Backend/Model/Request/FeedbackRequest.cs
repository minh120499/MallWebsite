namespace Backend.Model.Request;

public class FeedbackRequest
{
    public int? Id { get; set; }

    public string? Name { get; set; }

    public string? Message { get; set; }

    public string? Email { get; set; }
}