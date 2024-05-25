using Domain.Entities.Users;
using Infrastructure.Postgres.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Postgres.Mappings;

public class UserMapping : BaseEntityMapping<User>
{
    protected override string TableName => "users";

    public UserMapping(EntityTypeBuilder<User> builder) : base(builder)
    {
    }

    protected override void MapProperties()
    {
        Builder.Property(x => x.Email).HasColumnName("email").IsRequired();
        Builder.Property(x => x.Password).HasColumnName("password").IsRequired();
        Builder.Property(x => x.Key).HasColumnName("key").IsRequired();
    }

    protected override void MapIndexes() => Builder.HasIndex(x => new { x.Email, x.Active });

    protected override void MapForeignKeys()
    {
    }
}