using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Model.Entities;

[Table("Banners")]
public class Banner
{
    [Key]
    public int Id { get; set; }
    
    [Required(AllowEmptyStrings = false)]
    public string? Name { get; set; }
    
    public string? Image { get; set; }
    
    [Required]
    public int StoreId { get; set; }

    [ForeignKey("StoreId")]
    public Store? Store { get; set; }
    
    public int? Expire { get; set; }
    
    public DateTime? StartOn { get; set; }
    
    public DateTime? EndOn { get; set; }
    
    public string? Status { get; set; }
    public DateTime? CreateOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
}