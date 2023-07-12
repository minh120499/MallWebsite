using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;
using Backend.Model.Response;
using Backend.Repository;
using Backend.Utils;

namespace Backend.Service;

public class ProductsService
{
    private readonly IProductsRepository _productsRepository;

    public ProductsService(IProductsRepository productsRepository)
    {
        _productsRepository = productsRepository;
    }

    public async Task<TableListResponse<Product>> GetByFilter(FilterModel filters)
    {
        var products = await _productsRepository.GetByFilter(filters);
        var total = await _productsRepository.Count();
        return new TableListResponse<Product>()
        {
            Total = total,
            Limit = filters.Limit,
            Page = filters.Page,
            Data = products
        };
    }

    public async Task<Product> Create(ProductRequest request)
    {
        Validations.Product(request);

        var product = new Product()
        {
            Name = request.Name,
            Image = request.Image,
            Status = StatusConstraint.ACTIVE,
        };
        return await _productsRepository.Add(product);
    }
}