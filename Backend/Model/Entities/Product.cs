using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Model.Entities;

[Table("Products")]
public class Product
{
    [Key] public int Id { get; set; }
    public string? Code { get; set; }
    public string? Image { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Brand { get; set; }
    [Required] public int StoreId { get; set; }

    [ForeignKey("StoreId")] public Store? Store { get; set; }
    public List<ProductCategory>? ProductCategory { get; set; }
    public List<Variant> Variants { get; set; } = new List<Variant>();
    public string? Status { get; set; }
    public DateTime? CreateOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
}