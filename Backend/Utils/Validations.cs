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

        if (request.StoreId == 0)
        {
            errors.Add(new Dictionary<string, string>()
            {
                { "store_id", "store_id is not blank" }
            });
        }

        if (request.StartOn == null)
        {
            errors.Add(new Dictionary<string, string>()
            {
                { "start_on", "start_on is not blank" }
            });
        }
        
        if (request.EndOn != null && request.EndOn < request.StartOn)
        {
            errors.Add(new Dictionary<string, string>()
            {
                { "end_on", "end date must be greater than start date" }
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
        
        if (request.Type is not ("product" and "store"))
        {
            errors.Add(new Dictionary<string, string>()
            {
                { "type", "type must be product or store" }
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
        if (request.FullName is null or "")
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
        
        if (request.Message is null or "")
        {
            errors.Add(new Dictionary<string, string>()
            {
                { "message", "message is not blank" }
            });
        }
        
        if (request.Email is null or "")
        {
            errors.Add(new Dictionary<string, string>()
            {
                { "email", "email is not blank" }
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
        if (request.Source is null)
        {
            errors.Add(new Dictionary<string, string>()
            {
                { "name", "name is not blank" }
            });
        }
        
        if (request.OrdersLineItems == null || request.OrdersLineItems.Count == 0)
        {
            errors.Add(new Dictionary<string, string>()
            {
                { "order_line_item", "order_line_item is not blank" }
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
        if (request.Quantity < 1)
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

        if (request.FloorId == 0)
        {
            errors.Add(new Dictionary<string, string>()
            {
                { "floor_id", "floor_id is not blank" }
            });
        }

        if (request.CategoryId == 0)
        {
            errors.Add(new Dictionary<string, string>()
            {
                { "category_id", "category_id is not blank" }
            });
        }

        if (errors.Count > 0)
        {
            throw new FormValidationException(errors);
        }
    }
}