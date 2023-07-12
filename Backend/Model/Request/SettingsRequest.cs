using Backend.Model.Entities;

namespace Backend.Model.Request;

public class SettingsRequest
{
    public List<Facility> Facilities { set; get; } = new();
    public List<Floor> Floors { set; get; } = new();
}