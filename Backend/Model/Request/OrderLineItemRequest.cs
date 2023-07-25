using Backend.Model.Entities;

namespace Backend.Model.Request;

public class OrderLineItemRequest
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public Order? Order { get; set; }

    public int ProductId { get; set; }
    
    public Product? Product { get; set; }

    public string? ProductName { get; set; }
    
    public List<Variant>? Variants { get; set; }

    public float Price { get; set; }

    public int Quantity { get; set; }
}