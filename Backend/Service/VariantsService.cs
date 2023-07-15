using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;
using Backend.Model.Response;
using Backend.Repository;
using Backend.Utils;

namespace Backend.Service
{
    public class VariantsService
    {
        private readonly IVariantsRepository _variantsRepository;

        public VariantsService(IVariantsRepository variantsRepository)
        {
            _variantsRepository = variantsRepository;
        }

        public async Task<Variant> GetById(int variantId)
        {
            return await _variantsRepository.GetById(variantId);
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
                ProductId = request.ProductId,
                // Size = request.Size,
                // Color = request.Color,
            };
            return await _variantsRepository.Add(variant);
        }

        public async Task<Variant> Update(int variantId, VariantRequest request)
        {
            Validations.Variant(request);

            return await _variantsRepository.Update(variantId, request);
        }

        public async Task<bool> Delete(string ids)
        {
            var variantIds = ids.Split(',').Select(int.Parse).ToList();

            return await _variantsRepository.Delete(variantIds);
        }
    }
}