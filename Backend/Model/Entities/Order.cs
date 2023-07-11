using System.ComponentModel.DataAnnotations;

namespace Backend.Model.Entities;

public class Order
{
    [Key]
    public int Id { get; set; }
    
    public Store? Source { get; set; }
    
    public Employee? SaleBy { get; set; }
    
    public List<OrderLineItem>? OrdersLineItems { get; set; }

    public float? TotalPrice { get; set; }
    
    public string? Status { get; set; }

    public DateTime? CreateOn { get; set; }
    
    public DateTime? ModifiedOn { get; set; }
}