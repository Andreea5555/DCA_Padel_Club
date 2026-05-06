using DCA_Padel_Club.Core.Domain.Aggregates.Players;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DCA_Padel_Club.Infrastructure.EfcDomainModelPersistence.Configurations.Players;

public sealed class PlayerConfiguration : IEntityTypeConfiguration<Player>
{
    public void Configure(EntityTypeBuilder<Player> builder)
    {
        builder.ToTable("Players");

        builder.HasKey(player => player.Id);

        builder.Property(player => player.Id)
            .ValueGeneratedNever();

        builder.Property(player => player.FirstName)
            .HasConversion(
                name => name.Value,
                value => Name.Create(value).value)
            .HasMaxLength(25)
            .IsRequired();

        builder.Property(player => player.LastName)
            .HasConversion(
                name => name.Value,
                value => Name.Create(value).value)
            .HasMaxLength(25)
            .IsRequired();

        builder.Property(player => player.Email)
            .HasConversion(
                email => email.Value,
                value => Email.Create(value).value)
            .HasMaxLength(13)
            .IsRequired();

        builder.Property(player => player.Password)
            .HasConversion(
                password => password.Value,
                value => Password.Create(value).value)
            .IsRequired();

        builder.Property(player => player.ProfilePicture)
            .HasConversion(
                picture => picture.Value,
                value => ProfilePicture.Create(value).value)
            .IsRequired();

        // ...existing code...

        builder.Property(player => player.Blacklisted)
            .IsRequired();

        builder.Property(player => player.CooldownExpiresAt);
        builder.Property(player => player.QuarantineEndDate);
    }
}