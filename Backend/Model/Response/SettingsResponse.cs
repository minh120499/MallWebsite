using Backend.Model.Entities;

namespace Backend.Model.Response;

public class SettingsResponse
{
    public List<Facility>? Facilities { get; set; }
    public List<Floor>? Floors { get; set; }
}