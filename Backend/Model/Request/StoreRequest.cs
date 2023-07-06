namespace Backend.Model.Request;

public class StoreRequest
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Location { get; set; }

    public string? Status { get; set; }

    public DateTime? CreateOn { get; set; }

    public DateTime? ModifiedOn { get; set; }
}