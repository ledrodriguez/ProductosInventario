namespace Domain.Entities;

public abstract class BaseListedProjectionDto
{
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; init; }
    public string CreatedBy { get; init; }
    public DateTime LastUpdatedAt { get; init; }
    public string LastUpdatedBy { get; init; }
    public bool Active { get; init; }
}