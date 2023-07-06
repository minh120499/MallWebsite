﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Model.Entities;

[Table("Stores")]
public class Store
{
    [Key]
    public int Id { get; set; }
    
    [Required(AllowEmptyStrings = false)]
    public string? Name { get; set; }
    
    public string? Location { get; set; }
    
    public string? Status { get; set; }

    public DateTime? CreateOn { get; set; }
    
    public DateTime? ModifiedOn { get; set; }
}