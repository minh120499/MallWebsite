using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Backend.Model.Entities;

[Table("Employees")]
public class Employees : IdentityUser
{
    [Phone] public string? Phone { get; set; }

    [Required(AllowEmptyStrings = false)] 
    public string FullName { get; set; } = "";

    public string? Address { get; set; }

    public string? Status { get; set; }

    [Timestamp]
    [Required]
    public DateTime? CreateOn { get; set; }
    
    [Timestamp]
    [Required]
    public DateTime? ModifiedOn { get; set; }
}