using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Model.Entities;

[Table("Feedbacks")]
public class Feedback
{
    [Key]
    public int Id { get; set; }
    
    [Required(AllowEmptyStrings = false)]
    public string? Message { get; set; }

    [Required(AllowEmptyStrings = false)]
    public string? Email { get; set; }
    
    [Required(AllowEmptyStrings = false)]
    public string? Name { get; set; }
    
    public string? Status { get; set; }
    
    public DateTime? CreateOn { get; set; }
}