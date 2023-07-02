﻿using Backend.Exceptions;
using Backend.Model.Request;

namespace Backend.Utils;

public static class Validations
{
    public static void Banner(BannerRequest request)
    {
        var errors = new List<Dictionary<string, string>>();
        if (request.Name == null)
        {
            errors.Add(new Dictionary<string, string>()
            {
                { "name", "name is not null" }
            });
        }

        if (errors.Count > 0)
        {
            throw new FormValidationException(errors);
        }
    }
}