namespace Backend.Model.Response;

public class TableListResponse<T> : Metadata
{
    public ICollection<T>? Data { get; set; }
}