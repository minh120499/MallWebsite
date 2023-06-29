using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Model.Entities;

[Table("Facilitie")]
public class Facilities
{
    [Key]
    public int Id { get; set; }
    
    [Required(AllowEmptyStrings = false)]
    public string? Name { get; set; }

    [Timestamp]
    [Required]
    public DateTime? CreateOn { get; set; }
    
    [Timestamp]
    [Required]
    public DateTime? ModifiedOn { get; set; }
}