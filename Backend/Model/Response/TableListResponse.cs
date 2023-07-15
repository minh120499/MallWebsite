namespace Backend.Model.Response;

public class TableListResponse<T> : Metadata
{
    public List<T>? Data { get; set; }
}