using Backend.Exceptions;
using Backend.Model.Entities;
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

    public static void Category(CategoryRequest request)
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

    public static void Employee(EmployeeRequest request)
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

    public static void Facility(FacilityRequest request)
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

    public static void Feedback(FeedbackRequest request)
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

    public static void Floor(FloorRequest request)
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

    public static void Order(OrderRequest request)
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

    public static void OrderLineItem(OrderLineItemRequest request)
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

    public static void Product(ProductRequest request)
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

    public static void StoreProduct(StoreProductRequest request)
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

    public static void Variant(VariantRequest request)
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

    public static void Facility(Facility request)
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

    public static void Floor(Floor request)
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
}