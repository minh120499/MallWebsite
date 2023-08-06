using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Model.Entities;

[Table("ProductCategory")]
public class ProductCategory
{
    [Key]
    [Column(Order = 1)]
    public int ProductId { get; set; }

    [Key]
    [Column(Order = 2)]
    public int CategoryId { get; set; }
    public Product? Product { get; set; }
    public Category? Category { get; set; }
}