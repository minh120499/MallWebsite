namespace Backend.Model.Request;

public class BannerRequest
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public string? Image { get; set; }
    public int? Expire { get; set; }
    public int StoreId { get; set; }
    public DateTime? StartOn { get; set; }
    public DateTime? EndOn { get; set; }
    public string? Status { get; set; }
}