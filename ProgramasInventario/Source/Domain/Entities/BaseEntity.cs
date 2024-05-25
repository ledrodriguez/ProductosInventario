namespace Domain.Entities;

public abstract class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get;  set; }
    public string CreatedBy { get;  set; }
    public DateTime LastUpdatedAt { get;  set; }
    public string LastUpdatedBy { get;  set; }
    public bool Active { get;  set; }

    protected void SetCreate(string requestedBy)
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        CreatedBy = requestedBy;
        SetUpdate(requestedBy);
        Active = true;
    }

    protected void SetInactive(string requestedBy)
    {
        SetUpdate(requestedBy);
        Active = false;
    }

    protected void SetUpdate(string requestedBy)
    {
        LastUpdatedAt = DateTime.UtcNow;
        LastUpdatedBy = requestedBy;
    }
}