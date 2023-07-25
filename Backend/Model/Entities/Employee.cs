using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Backend.Model.Entities;

[Table("Employees")]
public class Employee : IdentityUser
{
    [Phone]
    public string? Phone { get; set; }

    [Required(AllowEmptyStrings = false)] 
    public string FullName { get; set; } = "";

    public string? Address { get; set; }
    
    [Required]
    public int StoreId { get; set; }

    [ForeignKey("StoreId")]
    public Store? Store { get; set; }

    public string? Status { get; set; }

    public DateTime? CreateOn { get; set; }
    
    public DateTime? ModifiedOn { get; set; }
}