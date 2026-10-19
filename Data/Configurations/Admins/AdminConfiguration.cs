using Astibosoft.Api.Domain.Admins;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Astibosoft.Api.Data.Configurations.Admins;

public sealed class AdminConfiguration : IEntityTypeConfiguration<Admin>
{
    public void Configure(EntityTypeBuilder<Admin> entity)
    {
        entity.ToTable("Admins");

        entity.HasKey(a => a.Id);

        entity.Property(a => a.Email)
            .HasMaxLength(320)
            .IsRequired();

        entity.Property(a => a.Name)
            .HasMaxLength(200)
            .IsRequired();

        entity.Property(a => a.PasswordHash)
            .HasMaxLength(200)
            .IsRequired();

        entity.HasIndex(a => a.Email)
            .IsUnique()
            .HasDatabaseName("IX_Admins_Email");
    }
}
