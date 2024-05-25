using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Postgres.Common;

public abstract class BaseEntityMapping<T> where T : BaseEntity
{
    protected readonly EntityTypeBuilder<T> Builder;
    protected abstract string TableName { get; }

    protected BaseEntityMapping(EntityTypeBuilder<T> builder)
    {
        Builder = builder;
        Map();
    }

    protected abstract void MapProperties();
    protected abstract void MapIndexes();
    protected abstract void MapForeignKeys();

    private void Map()
    {
        MapTableName();
        MapBaseProperties();
        MapPrimaryKey();
        MapProperties();
        MapIndexes();
        MapForeignKeys();
    }

    private void MapTableName() => Builder.ToTable(TableName);

    private void MapBaseProperties()
    {
        Builder.Property(x => x.Id).HasColumnName("id").IsRequired();
        Builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
        Builder.Property(x => x.CreatedBy).HasColumnName("created_by").IsRequired();
        Builder.Property(x => x.LastUpdatedAt).HasColumnName("last_updated_at").IsRequired();
        Builder.Property(x => x.LastUpdatedBy).HasColumnName("last_updated_by").IsRequired();
        Builder.Property(x => x.Active).HasColumnName("active").IsRequired();
    }

    private void MapPrimaryKey() => Builder.HasKey(x => x.Id);
}