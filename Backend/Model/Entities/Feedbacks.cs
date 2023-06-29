﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Model.Entities;

[Table("Feedback")]
public class Feedbacks
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
    
    [Timestamp]
    [Required]
    public DateTime? CreateOn { get; set; }
}