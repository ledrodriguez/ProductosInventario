namespace Presentation.Contracts.Models.Users;

/// <summary>
/// The user details.
/// </summary>
public class UserModel
{
    /// <summary>
    /// Email.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// ID.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Date of creation. 
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Author of creation.
    /// </summary>
    public string CreatedBy { get; set; }

    /// <summary>
    /// Date of last update.
    /// </summary>
    public DateTime LastUpdatedAt { get; set; }

    /// <summary>
    /// Author of last update.
    /// </summary>
    public string LastUpdatedBy { get; set; }

    /// <summary>
    /// Status.
    /// </summary>
    public bool Active { get; set; }
}