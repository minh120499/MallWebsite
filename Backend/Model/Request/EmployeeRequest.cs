using Backend.Model.Entities;

namespace Backend.Model.Request;

public class EmployeeRequest
{
    public string? Phone { get; set; }

    public string FullName { get; set; } = "";

    public string? Address { get; set; }
    public string? Email { get; set; }

    public int StoreId { get; set; }

    public Store? Store { get; set; }

    public string? Status { get; set; }
}