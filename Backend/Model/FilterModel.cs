using Backend.Utils;

namespace Backend.Model;

public class FilterModel
{
    public string? Query { get; set; }
    public int Limit { get; set; } = 10;
    public int Page { get; set; } = 1;
    public string Status { get; set; } = StatusConstraint.ACTIVE;
    public List<int> StoreId { get; set; } = new List<int>();
}