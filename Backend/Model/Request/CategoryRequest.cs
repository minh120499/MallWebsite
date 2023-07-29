namespace Backend.Model.Request;

public class CategoryRequest
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public string? Image { get; set; }

    public IFormFile? FormFile { get; set; }
    public string? Status { get; set; }
    public string? Type { get; set; }
}