using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;

namespace Backend.Repository;

public interface IVariantsRepository
{
    Task<Variant> GetById(int variantId);
    Task<List<Variant>> GetByFilter(FilterModel filters);
    Task<Variant> Add(Variant variant);
    Task<Variant> Update(int variantId, VariantRequest request);
    Task<int> Count();
    Task<bool> Delete(List<int> ids);
}