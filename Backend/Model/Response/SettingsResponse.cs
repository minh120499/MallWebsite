using Backend.Model.Entities;

namespace Backend.Model.Response;

public class SettingsResponse
{
    public ICollection<Facility>? Facilities;
    public ICollection<Floor>? Floors;
}