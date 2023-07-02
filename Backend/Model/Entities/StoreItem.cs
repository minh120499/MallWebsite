using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Model.Entities;

[Table("StoreItems")]
public class StoreItem
{
    [Key]
    public int Id { get; set; }
    
    [Required(AllowEmptyStrings = false)]
    public string? Name { get; set; }
    
    [Required]
    public int StoreId { get; set; }

    [ForeignKey("StoreId")]
    public Store? Store { get; set; }
    
    public int? Available { get; set; }
    
    public int? Price { get; set; }

    public string? Status { get; set; }

    [Timestamp]
    [Required]
    public DateTime? CreateOn { get; set; }
    
    [Timestamp]
    [Required]
    public DateTime? ModifiedOn { get; set; }
}