using Backend.Model.Entities;

namespace Backend.Model.Response;

public class StoreProductResponse: Product
{
    public Store? Store { get; set; }
    // public float? Price { get; set; }
    // public int? InStock { get; set; }
}