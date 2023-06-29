using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Model.Entities;

[Table("Category")]
public class Categories
{
    [Key]
    public int Id { get; set; }
    
    [Required(AllowEmptyStrings = false)]
    public string? Name { get; set; }
    
    public string? Image { get; set; }
    
    public string? Status { get; set; }

    [Timestamp]
    [Required]
    public DateTime? CreateOn { get; set; }
    
    [Timestamp]
    [Required]
    public DateTime? ModifiedOn { get; set; }
}