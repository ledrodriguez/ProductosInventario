using Domain.Entities.Product;
using Infrastructure.Postgres.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Postgres.Mappings;

public class ProductMapping : BaseEntityMapping<ProductEntity>
{
    protected override string TableName => "products";

    public ProductMapping(EntityTypeBuilder<ProductEntity> builder) : base(builder)
    {
    }

    protected override void MapProperties()
    {
        Builder.Property(x => x.Id).HasColumnName("id").IsRequired();
        Builder.Property(x => x.Quantity).HasColumnName("quantity").IsRequired();
    }

    protected override void MapIndexes() => Builder.HasIndex(x => new { x.Id, x.Active });

    protected override void MapForeignKeys()
    {
    }
}