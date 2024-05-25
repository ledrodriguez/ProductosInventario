namespace Domain.Entities.Product;

public class ProductEntity : BaseEntity
{
    public int Quantity { get; private set; }

    private ProductEntity()
    {
    }

    public ProductEntity(int id, int quantity)
    {
        Quantity = quantity;
        this.Id = ToGuid(id);
        this.Active = true;
        this.CreatedAt = DateTime.UtcNow;
        this.CreatedBy = "admin";
        this.LastUpdatedAt = DateTime.UtcNow;
        this.LastUpdatedBy = "admin";
    }

    public void Inactivate(string requestedBy) => SetInactive(requestedBy);

    public void Update(string name, int quantity, string requestedBy)
    {
        SetUpdate(requestedBy);
        Quantity += quantity;
    }

    private Guid ToGuid(int value) {
        byte[] bytes = new byte[16];
        BitConverter.GetBytes(value).CopyTo(bytes, 0);
        return new Guid(bytes);
    }
}