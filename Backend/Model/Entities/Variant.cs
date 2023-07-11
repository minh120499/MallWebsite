using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Model.Entities;

[Table("Variants")]
public class Variant
{
    [Key]
    public int Id { get; set; }
    
    public int ProductId { get; set; }

    [ForeignKey("ProductId")]
    public Product? Product { get; set; }
    
    public string? Image { get; set; }
    
    public string? Code { get; set; }
    
    [Required]
    public string? Name { get; set; }
    
    public string? Options { get; set; }

    public string? Description { get; set; }

    public string? Status { get; set; }

    public DateTime? CreateOn { get; set; }
    
    public DateTime? ModifiedOn { get; set; }   
}