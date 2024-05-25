namespace Application.Services.Products;

using Application.DTOs.inventario;
using Domain.Entities.Product;

public interface IProductRegister {

    public Task Register(ProductoDTO producto);

}