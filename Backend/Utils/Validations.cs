using Backend.Exceptions;
using Backend.Model.Request;

namespace Backend.Utils;

public static class Validations
{
    public static void Banner(BannerRequest request)
    {
        var errors = new List<Dictionary<string, string>>();
        if (request.Name is null or "")
        {
            errors.Add(new Dictionary<string, string>()
            {
                { "name", "name is not blank" }
            });
        }

        if (errors.Count > 0)
        {
            throw new FormValidationException(errors);
        }
    }
    
    public static void Store(StoreRequest request)
    {
        var errors = new List<Dictionary<string, string>>();
        if (request.Name is null or "")
        {
            errors.Add(new Dictionary<string, string>()
            {
                { "name", "name is not blank" }
            });
        }

        if (errors.Count > 0)
        {
            throw new FormValidationException(errors);
        }
    }
    
    public static void StoreItem(StoreItemRequest request)
    {
        var errors = new List<Dictionary<string, string>>();
        if (request.Name is null or "")
        {
            errors.Add(new Dictionary<string, string>()
            {
                { "name", "name is not blank" }
            });
        }

        if (errors.Count > 0)
        {
            throw new FormValidationException(errors);
        }
    }
}