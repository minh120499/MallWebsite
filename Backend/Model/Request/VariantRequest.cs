using Backend.Model.Entities;

namespace Backend.Model.Request;

public class VariantRequest
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public Product? Product { get; set; }

    public string? Image { get; set; }
    public IFormFile? FormFile { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? Options { get; set; }

    public string? Description { get; set; }

    public string? Status { get; set; }
}