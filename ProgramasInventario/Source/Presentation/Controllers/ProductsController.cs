using Application.Services.Users;
using Domain.Entities;
using Domain.Entities.Users;
using Domain.Entities.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Contracts.Requests.Users;
using Presentation.Contracts.Responses.Users;
using Presentation.Contracts.Models.Producto;
using Application.Services.Products;

namespace Presentation.Controllers;

public class ProductsController : BaseController{

    private readonly IBaseEntityRepository<User> _repositoryUser;
    private readonly IBaseEntityRepository<ProductEntity> _repositoryProducts;

    private readonly IProductRegister _productRegister;

    public ProductsController(
        IBaseEntityRepository<User> repositoryUser,
        IBaseEntityRepository<ProductEntity> repositoryProducts,
        IProductRegister productRegister
    ): base(repositoryUser) {
        _repositoryProducts = repositoryProducts;
        _repositoryUser = repositoryUser;
        _productRegister = productRegister;
    }

    [HttpPost(nameof(AgregarProducto))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> AgregarProducto([FromBody] RegisterProductRequest request)
    {
        await ValidateJwtToken();
        // llamar al repositorio(repositoryProducts) de productos para guardar un nuvo registro
        await _productRegister.Register(request.ConvertToDto());
        return Ok(new { Message = $"Product with Id {request.id.ToString()} registered." });
    }

    [HttpPost(nameof(QuitarProducto))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public string QuitarProducto([FromBody] ProductModel product)
    {
        // llamar al repositorio(repositoryProducts) de productos para eliminar un nuevo registro 
        //_repositoryProducts.DeleteOne(new ProductEntity(product.Nombre, product.Quantity, product.ToString()));
        return "Producto eliminado";
    }

    [HttpGet(nameof(ConsultarProducto))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public string ConsultarProducto([FromBody] ProductModel product)
    {
        // llamar al repositorio(repositoryProducts) de productos para consultar un registro 
        //_repositoryProducts.DeleteOne(new ProductEntity(product.Nombre, product.Quantity, product.ToString()));
        return "Producto consultado";
    }


}