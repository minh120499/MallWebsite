using Backend.Model.Entities;

namespace Backend.Model.Request;

public class StoreRequest
{
    public int Id { get; set; }

    public string? Name { get; set; }
    public string? Image { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }

    public int FloorId { get; set; }

    public Floor? Floor { get; set; }

    public int CategoryId { get; set; }

    public Category? Category { get; set; }

    public List<Facility>? Facilities { get; set; }
    public string? FacilityIds { get; set; }

    public List<Banner>? Banners { get; set; }
    public string? BannersIds { get; set; }

    public string? Description { get; set; }
    public string? Status { get; set; }
}