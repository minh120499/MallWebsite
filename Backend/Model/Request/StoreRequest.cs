using Backend.Model.Entities;

namespace Backend.Model.Request;

public class StoreRequest
{
    public int Id { get; set; }
    
    public string? Name { get; set; }
    
    public int FloorId { get; set; }

    public Floor? Floor { get; set; }
    
    public int CategoryId { get; set; }

    public Category? Category { get; set; }
    
    public List<Facility>? Facilities { get; set; }
    
    public List<Banner>? Banners { get; set; }
    
    public string? Description { get; set; }
    public string? Status { get; set; }
}