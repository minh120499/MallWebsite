namespace Backend.Model;

public class FilterModel
{
    public string? Query { get; set; }
    public int Limit { get; set; } = 10;
    public int Page { get; set; } = 1;
}