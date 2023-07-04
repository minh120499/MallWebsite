﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Model.Entities;

[Table("StoreBanners")]
public class StoreBanner
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int StoreId { get; set; }

    [ForeignKey("StoreId")]
    public Store? Store { get; set; }
    
    [Required]
    public int BannerId { get; set; }

    [ForeignKey("BannerId")]
    public Banner? Banner { get; set; }
    
    public int? Expire { get; set; }
    
    [Timestamp]
    [Required]
    public DateTime? StartOn { get; set; }
    
    [Timestamp]
    [Required]
    public DateTime? EndOn { get; set; }

    
    public string? Status { get; set; }

    [Timestamp]
    [Required]
    public DateTime? CreateOn { get; set; }
    
    [Timestamp]
    [Required]
    public DateTime? ModifiedOn { get; set; }
}