using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Model.Entities;

[Table("StoreProducts")]
public class StoreProduct
{
    [Key] 
    public int Id { get; set; }

    [Required] 
    public int StoreId { get; set; }

    [ForeignKey("StoreId")]
    public Store? Store { get; set; }

    [ForeignKey("ProductId")]
    public int? ProductId { get; set; }

    public Product? Product { get; set; }

    public List<Variant>? Variants { get; set; }
    public float? Price { get; set; }

    public int? InStock { get; set; }
    public string? Status { get; set; }
    public DateTime? CreateOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
}