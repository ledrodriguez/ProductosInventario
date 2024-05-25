using System.ComponentModel.DataAnnotations;
using Application.DTOs.inventario;
using Application.DTOs.Users;
using Domain.Entities.Product;

namespace Presentation.Contracts.Requests.Users;

/// <summary>
/// Register user request.
/// </summary>
public class RegisterProductRequest
{
    /// <summary>
    /// Valid formatted email.
    /// </summary>
    [Required]
    public int id { get; init; }

    /// <summary>
    /// Valid formatted password.
    /// At least one lower case letter, one upper case letter and one number.
    /// Must have between 8 and 16 characters.
    /// </summary>
    [Required]
    public int quantity { get; init; }

    public ProductoDTO ConvertToDto() => new()
    {
        Id = id,
        Quantity = quantity
    };
}