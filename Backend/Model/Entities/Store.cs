using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Model.Entities;

[Table("Stores")]
public class Store
{
    [Key]
    public int Id { get; set; }
    
    [Required(AllowEmptyStrings = false)]
    public string? Name { get; set; }
    public string? Image { get; set; }
    
    [Required]
    [Phone]
    public string? Phone { get; set; }
    
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
    
    [Required]
    public int FloorId { get; set; }

    [ForeignKey("FloorId")]
    public Floor? Floor { get; set; }
    
    [Required]
    public int CategoryId { get; set; }

    [ForeignKey("CategoryId")]
    public Category? Category { get; set; }
    
    public string? Facilities { get; set; }
    
    public List<Banner> Banners { get; set; } = new List<Banner>();
    
    public string? Description { get; set; }
    public string? Status { get; set; }

    public List<Product>? Products { get; set; } = new List<Product>();
    public DateTime? CreateOn { get; set; }
    
    public DateTime? ModifiedOn { get; set; }
}