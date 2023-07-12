using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;
using Backend.Model.Response;
using Backend.Repository;
using Backend.Utils;

namespace Backend.Service;

public class VariantsService
{
    private readonly IVariantsRepository _variantsRepository;

    public VariantsService(IVariantsRepository variantsRepository)
    {
        _variantsRepository = variantsRepository;
    }

    public async Task<TableListResponse<Variant>> GetByFilter(FilterModel filters)
    {
        var variants = await _variantsRepository.GetByFilter(filters);
        var total = await _variantsRepository.Count();
        return new TableListResponse<Variant>()
        {
            Total = total,
            Limit = filters.Limit,
            Page = filters.Page,
            Data = variants
        };
    }

    public async Task<Variant> Create(VariantRequest request)
    {
        Validations.Variant(request);

        var variant = new Variant()
        {
            Name = request.Name,
            Image = request.Image,
            Status = StatusConstraint.ACTIVE,
        };
        return await _variantsRepository.Add(variant);
    }
}