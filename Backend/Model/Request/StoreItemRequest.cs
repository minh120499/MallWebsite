namespace Backend.Model.Request;

public class StoreItemRequest
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int StoreId { get; set; }

    public int? Available { get; set; }

    public int? Price { get; set; }

    public string? Status { get; set; }

    public DateTime? CreateOn { get; set; }

    public DateTime? ModifiedOn { get; set; }
}