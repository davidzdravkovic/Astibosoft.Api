using Asibosoft.Api.Domain.Contacts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Asibosoft.Api.Data.Configurations.Contacts;

public sealed class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> entity)
    {
        entity.ToTable("Contacts");

        entity.HasKey(c => c.Id);

        entity.Property(c => c.Name)
            .HasMaxLength(200)
            .IsRequired();

        entity.Property(c => c.Email)
            .HasMaxLength(320)
            .IsRequired();

        entity.Property(c => c.Company)
            .HasMaxLength(300);

        entity.Property(c => c.Service)
            .HasMaxLength(200);

        entity.Property(c => c.Message)
            .HasMaxLength(1500)
            .IsRequired();

        entity.Property(c => c.ClientIp)
            .HasMaxLength(64);

        entity.HasIndex(c => c.CreatedAt)
            .HasDatabaseName("IX_Contacts_CreatedAt");

        entity.HasIndex(c => c.Email)
            .HasDatabaseName("IX_Contacts_Email");
    }
}
