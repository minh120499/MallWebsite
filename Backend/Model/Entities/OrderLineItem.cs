using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Model.Entities;

[Table("OrderLineItems")]
public class OrderLineItem
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int OrderId { get; set; }

    [ForeignKey("OrderId")]
    public Order? Order { get; set; }

    [ForeignKey("ProductId")]
    public int ProductId { get; set; }
    
    public Product? Product { get; set; }

    public string? ProductName { get; set; }
    
    public List<Variant>? Variants { get; set; }

    public float Price { get; set; }

    public int Quantity { get; set; }
}