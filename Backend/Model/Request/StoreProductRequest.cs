using Backend.Model.Entities;

namespace Backend.Model.Request;

public class StoreProductRequest
{
    public int Id { get; set; }

    public int StoreId { get; set; }

    public Store? Store { get; set; }

    public int? ProductId { get; set; }

    public Product? Product { get; set; }

    public List<Variant>? Variants { get; set; }
    public float? Price { get; set; }

    public int? InStock { get; set; }
    public string? Status { get; set; }
    public DateTime? CreateOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
}