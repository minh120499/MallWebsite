using Backend.Utils;

namespace Backend.Model;

public class FilterModel
{
    public string? Query { get; set; }
    public int Limit { get; set; } = 10;
    public int Page { get; set; } = 1;
    public string? Status { get; set; }
    public List<int> Ids { get; set; } = new List<int>();
    public int StoreId { get; set; }
    public string? Type { get; set; }
    
    public string? Category { get; set; }
    public int? CategoryId { get; set; }
}