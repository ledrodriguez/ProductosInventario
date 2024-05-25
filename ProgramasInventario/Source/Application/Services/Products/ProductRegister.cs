using Application.Common.Exceptions;
using Application.DTOs.inventario;
using Application.DTOs.Users;
using Application.Services.Users.Security;
using Domain.Entities;
using Domain.Entities.Product;
using Microsoft.Extensions.Logging;

namespace Application.Services.Products;

public class ProductRegister : IProductRegister
{
    private readonly IBaseEntityRepository<ProductEntity> _repository;
    private readonly ILogger<ProductRegister> _logger;

    public ProductRegister(IBaseEntityRepository<ProductEntity> repository, ILogger<ProductRegister> logger)
    {
        _repository = repository;
        _logger = logger;
    }


    public async Task Register(ProductoDTO producto)
    {
        var id = producto.Id;

        if (await _repository.ExistsBy(x => x.Id.ToString() == id.ToString()))
            throw new UserAlreadyRegisteredException($"Product with Id {id} already registered.");

        await _repository.InsertOne(new ProductEntity(id, producto.Quantity));
    }
}