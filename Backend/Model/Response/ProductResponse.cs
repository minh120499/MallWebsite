using Backend.Model.Entities;

namespace Backend.Model.Response;

public class ProductResponse
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public string? Image { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Brand { get; set; }
    public List<string>? Categories { get; set; }
    public List<Variant>? Variants { get; set; }
    public string? Status { get; set; }
    public DateTime? CreateOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
}